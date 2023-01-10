package controller

import (
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"
	"strconv"
	"strings"

	log "evn/lib/logger"
	"evn/lib/utils"
	"evn/server/constant"
	"evn/server/dao"

	"github.com/gofiber/fiber/v2"
	"github.com/spf13/viper"
)

func (ctrl *Controller) JiraWebHook(ctx *fiber.Ctx) error {
	//log.Get().Infof("JiraWebHook Start parser body, data: %s", string(ctx.Body()))

	req := dao.JiraWebhookData{}
	err := ctx.BodyParser(&req)
	if err != nil {
		log.Get().Errorf("JiraWebHook parser body err: %+v, data: %s", err, string(ctx.Body()))

		return utils.WriteError(ctx, fiber.StatusBadRequest, err)
	}
	log.Get().Infof("JiraWebHook self: %s, status:%+v", req.Issue.Self, req.Issue.Fields.Status)

	if req.Issue.Fields.Status.Name != KClosed {

		return utils.WriteError(ctx, fiber.StatusBadRequest, errors.New("Status.Name != KClosed"))
	}

	// get SDP reqest ID by Jira ID from DB
	maping, err := dao.GetSDPByJira(req.Issue.ID)
	if err != nil {
		log.Get().Errorf("JiraWebHook GetSDPByJira issueID:%s", req.Issue.ID)

		return utils.WriteError(ctx, fiber.StatusBadRequest, err)
	}

	resolution := ""
	if req.Issue.Fields.Resolution != nil {
		resolution = fmt.Sprintf(`
		<div>Kết Quả: %s<br /></div>
		<div>Nguyên Nhân: %s<br /></div>
		<div>Cách Giải Quyết: %s<br /></div>`, req.Issue.Fields.Resolution.Name, req.Issue.Fields.Customfield10018, req.Issue.Fields.Customfield10019)
	}
	// update SDP request status
	err = UpdateSDPRequestStatus(maping.SdpID, resolution, req.Issue.Fields.Project.Key, &req.Issue.Fields)
	if err != nil {
		log.Get().Errorf("JiraWebHook UpdateSDPRequestStatus err: %+v, SdpID: %s", err, maping.SdpID)

		return utils.WriteError(ctx, fiber.StatusBadRequest, err)
	}

	log.Get().Infof("JiraWebHook Done with no error, IDjira: %s, IDsdp:%s", req.Issue.ID, maping.SdpID)

	return utils.WriteSuccessEmptyContent(ctx)
}

func UpdateSDPRequestStatus(sdpReqID, resolution, jiraKey string, fields *dao.Fields) error {

	endpoint := KUrlUpdateSdpRequest + strings.TrimSpace(sdpReqID)

	var udfFields *dao.SdpUdfFields
	if jiraKey == constant.KProjects[constant.KSdpTemplateERP] {
		//log.Get().Infof("JiraWebHook UpdateSDPRequestStatus ERP:%+v", fields)

		udfFields = &dao.SdpUdfFields{
			// UdfPick2712: dao.CreateSDPCustomfield(fields.Customfield16760),
			// UdfPick2713: dao.CreateSDPCustomfield(fields.Customfield17860),
			// UdfPick2714: dao.CreateSDPCustomfield(fields.Customfield16562),

			UdfPick2704:  dao.CreateSDPCustomfield(fields.Customfield11761),
			UdfSline3907: dao.CreateSDPCustomfield(fields.Customfield13164),
			UdfPick3908:  dao.CreateSDPCustomfield(fields.Customfield11961),
			UdfPick3911:  dao.CreateSDPCustomfield(fields.Customfield14860),
			UdfMline4206: fields.Customfield10018,
			UdfMline4207: fields.Customfield10019,
		}
	}

	inputData := &dao.SdpInputData{
		Request: dao.SdpRequest{
			Resolution: dao.CreateResolution(resolution),
			Status:     dao.CreateStatus("Resolved"),
			UdfFields:  udfFields,
		},
	}

	rawInput, _ := json.Marshal(inputData)
	data := url.Values{}
	data.Set("input_data", string(rawInput))

	client := &http.Client{}
	r, err := http.NewRequest("PUT", endpoint, strings.NewReader(data.Encode())) // URL-encoded payload
	if err != nil {
		log.Get().Errorf("UpdateSDPRequestStatus NewRequest err:%s, url:%s, data:,%s", err, endpoint, data)
		return err
	}
	r.Header = http.Header{
		"authtoken": []string{viper.GetString("SDP.token")},
	}
	r.Header.Add("Content-Type", "application/x-www-form-urlencoded")
	r.Header.Add("Content-Length", strconv.Itoa(len(data.Encode())))

	resp, err := client.Do(r)
	if err != nil {
		log.Get().Errorf("UpdateSDPRequestStatus Do err:%s, url:%s, data:,%s", err, endpoint, data)
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		bodyText, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			log.Get().Errorf("UpdateSDPRequestStatus bodyText ReadAll spdID:%s, err:%s", endpoint, err)

			return err
		}
		log.Get().Errorf("UpdateSDPRequestStatus StatusCode :%d, body:%s", resp.StatusCode, string(bodyText))

		return errors.New("UpdateSDPRequestStatus resp.StatusCode != http.StatusCreated")
	}

	return nil
}
