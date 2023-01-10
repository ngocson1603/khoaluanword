/*
 * Copyright © 2021 Dio♦
 * All rights reserved.
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file.
 * ======
 * Created At:    Tuesday, 25th January 2022 11:57:52 pm
 * Last Modified: Thursday, 25th August 2022 5:24:48 pm
 * ======
 */

package controller

import (
	"bytes"
	"encoding/json"
	"errors"
	log "evn/lib/logger"
	"evn/lib/utils"
	"evn/server/constant"
	"evn/server/dao"
	"fmt"
	"io"
	"io/ioutil"
	"net/http"
	"net/url"
	"strconv"
	"strings"

	"github.com/gofiber/fiber/v2"
	"github.com/spf13/viper"
)

func (ctrl *Controller) SdpCreateJiraIssue(ctx *fiber.Ctx) error {

	requestID := ctx.Query("req_id", "")
	if requestID == "" {
		log.Get().Errorf("SdpWebhook validate requestID:%s", requestID)

		return utils.WriteError(ctx, fiber.StatusBadRequest, errors.New("request id invalid"))
	}
	reqID := strings.TrimSpace(requestID)

	log.Get().Infof("SdpWebhook =====> Start: reqID [%s] ", reqID)

	// get Jira ID by SDP ID from DB
	maping, err := dao.GetJiraBySDP(reqID)
	if err != nil {
		log.Get().Errorf("SdpWebhook GetJiraBySDP err:%v", err)
	}
	if maping != nil {
		err := fmt.Sprintf("issue da duoc tao tren jira voi id: %s", maping.JiraID)
		return utils.WriteError(ctx, fiber.StatusBadRequest, errors.New(err))
	}

	// Get SDP request Detail
	requestDetail, err := GetSDPRequestDetail(reqID)
	if err != nil {
		log.Get().Errorf("SdpWebhook GetSDPRequestDetail err:%v", err)

		return utils.WriteError(ctx, fiber.StatusInternalServerError, err)
	}

	log.Get().Infof("SdpWebhook sdp request Detail: %+v", requestDetail)

	// SDP Can o trang thai [In Progress]
	if requestDetail.Request.Status.Name != "In Progress" {
		log.Get().Errorf("SdpWebhook requestDetail.Request.Status.Name :%s", requestDetail.Request.Status.Name)

		return utils.WriteError(ctx, fiber.StatusBadRequest, errors.New("request [Status] can o trang thai [In Process]"))
	}

	// Create Jira Issue
	jiraResult, err := ctrl.CreateJiraIssueTemplate1(requestDetail)
	if err != nil || jiraResult.ID == "" {
		log.Get().Errorf("SdpWebhook CreateJiraIssueTemplate1 err:%v", err)

		return utils.WriteError(ctx, fiber.StatusInternalServerError, err)
	}

	log.Get().Infof("SdpWebhook jira create issue Result: %+v", jiraResult)

	// Set jira link in sdp
	err = UpdateJiraLinkInSDP(reqID, jiraResult.Key)
	if err != nil {
		log.Get().Errorf("SdpWebhook UpdateJiraLinkInSDP err:%v", err)

		return utils.WriteError(ctx, fiber.StatusInternalServerError, err)
	}
	log.Get().Infof("SdpWebhook UpdateJiraLinkInSDP Done: requestID %s, Link:%s", reqID, jiraResult.Self)

	// save issue info to DB
	err = dao.Insert(&dao.SDPAndJiraMaping{
		SdpID:  reqID,
		JiraID: strings.TrimSpace(jiraResult.ID),
	})
	if err != nil {
		log.Get().Errorf("SdpWebhook Insert ids err:%v", err)
		return ctx.SendString("insert error")
	}
	log.Get().Infof("SdpWebhook =====> With no error Done: [%s] ", reqID)

	// Return
	return utils.WriteSuccessEmptyContent(ctx)
}

// Functions
//
//
//
func GetSDPRequestDetail(reqID string) (*dao.Template1, error) {

	url := KUrlGetSdpRequest + reqID

	client := http.Client{}
	req, err := http.NewRequest("GET", url, nil)
	if err != nil {
		log.Get().Errorf("GetSDPRequestDetail NewRequest url:%s, err:%s", url, err)

		return nil, err
	}

	req.Header = http.Header{
		"authtoken": []string{viper.GetString("SDP.token")},
	}

	resp, err := client.Do(req)
	if err != nil {
		log.Get().Errorf("GetSDPRequestDetail Do url:%s, err:%s", url, err)

		return nil, err
	}
	defer resp.Body.Close()

	bodyBytes, err := io.ReadAll(resp.Body)
	if err != nil {
		log.Get().Errorf("GetSDPRequestDetail ReadAll url:%s, err:%s", url, err)

		return nil, err
	}

	requestDetail := &dao.Template1{}
	err = json.Unmarshal(bodyBytes, &requestDetail)
	if err != nil {
		log.Get().Errorf("GetSDPRequestDetail Unmarshal requestDetail err:%v", err)

		return nil, err
	}

	return requestDetail, err
}

func (ctrl *Controller) CreateJiraIssueTemplate1(templ *dao.Template1) (*dao.CreateJiraResult, error) {

	sdpRequestLink := KUrlOfSdpRequest + templ.Request.ID

	jiraProject := dao.CreateProject(constant.KProjects, templ.Request.Template.ID)
	if jiraProject == nil {
		return nil, fmt.Errorf("sdp project no maping with jira project, got sdp ID: %s", templ.Request.Template.ID)
	}
	fields := &dao.JiraFields{
		Project:     jiraProject,
		Summary:     dao.CreateSummary(templ.Request.Subject),
		Issuetype:   dao.CreateIssuetype(),
		Description: dao.CreateDescription(templ.Request.Description),
		Priority:    dao.CreatePriority(templ.Request.Priority.Name),
		Components: dao.CreateComponents(
			jiraProject.Key,
			templ.Request.Subcategory.Name, // component ERP
			templ.Request.UdfFields.UdfPick3001,
			templ.Request.UdfFields.UdfPick3002,
			templ.Request.UdfFields.UdfPick3003,
			templ.Request.UdfFields.UdfPick3004,
			templ.Request.UdfFields.UdfPick3005,
			templ.Request.UdfFields.UdfPick3006,
			templ.Request.UdfFields.UdfPick3007,
			templ.Request.UdfFields.UdfPick3008,
			templ.Request.UdfFields.UdfPick3009,
		),
		Customfield16960: dao.CreateCustomfield16960(templ.Request.UdfFields.UdfSline2709),
		Customfield10014: dao.CreateCustomfield10014(templ.Request.UdfFields.UdfSline2708),
		Customfield18760: dao.CreateCustomfield18760(sdpRequestLink),
		Customfield10012: dao.CreateCustomfield10012(templ.Request.UdfFields.UdfPick2706, templ.Request.UdfFields.UdfPick2707),
		Customfield13260: dao.CreateCustomfieldChucNangCMIS(jiraProject.Key, templ.Request.Subcategory.Name, templ.Request.Item.Name),
		Customfield15460: dao.CreateCustomfieldChucNangDOFFICE(jiraProject.Key, templ.Request.Subcategory.Name, templ.Request.Item.Name),
		//Customfield13660: dao.CreateCustomfieldChucNangDTXD(jiraProject.Key, templ.Request.Subcategory.Name, templ.Request.Item.Name),
		Customfield10013: dao.CreateCustomfieldChucNangHRMS(jiraProject.Key, templ.Request.Subcategory.Name, templ.Request.Item.Name),
		Customfield13661: dao.CreateCustomfieldChucNangPortal(jiraProject.Key, templ.Request.Subcategory.Name, templ.Request.Item.Name),
		Customfield18060: dao.CreateCustomfieldPhanHeIMIS2(jiraProject.Key, templ.Request.Subcategory.Name, ""),
		Customfield11060: dao.CreateCustomfieldSelectCF(templ.Request.Mode.Name),
		Customfield11761: dao.CreateCustomfieldSelectCF(templ.Request.UdfFields.UdfPick2704),
		Customfield17860: dao.CreateCustomfieldSelectCF(templ.Request.UdfFields.UdfPick2713),
		Customfield16760: dao.CreateCustomfieldSelectCF(templ.Request.UdfFields.UdfPick2712),
		Customfield16562: dao.CreateCustomfieldSelectCF(templ.Request.UdfFields.UdfPick2714),
		Customfield10262: dao.CreateCustomfieldSelectCF(templ.Request.UdfFields.UdfPick4203),
		Customfield19560: dao.CreateCustomfieldSelectCF(templ.Request.UdfFields.UdfPick3906),
		Customfield10769: dao.CreateCustomfield10769(jiraProject.Key, templ.Request.Item.Name),
	}

	err := dao.JiraValidate(fields)
	if err != nil {
		return nil, err
	}

	return CreateJiraIssue(fields)
}

func CreateJiraIssue(fields *dao.JiraFields) (*dao.CreateJiraResult, error) {

	//url := viper.GetString("Jira.host") + "/rest/api/2/issue"
	url := KUrlCreateJiraIssue
	// Body json
	data := dao.Payload{
		Update: dao.Update{},
		Fields: *fields,
	}
	payloadBytes, err := json.Marshal(data)
	if err != nil {
		log.Get().Errorf("CreateJiraIssue Marshal to payloadBytes, err:%s", err)

		return nil, err
	}
	log.Get().Infof("CreateJiraIssue data: %s", string(payloadBytes))

	body := bytes.NewReader(payloadBytes)

	// Add Request
	req, err := http.NewRequest("POST", url, body)
	if err != nil {
		log.Get().Errorf("CreateJiraIssue ReadAll err:%s, spdID:%s, Body:%s", url, err, payloadBytes)

		return nil, err
	}
	req.SetBasicAuth(viper.GetString("Jira.username"), viper.GetString("Jira.password"))
	req.Header.Set("Accept", "application/json")
	req.Header.Set("Content-Type", "application/json")

	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		log.Get().Errorf("CreateJiraIssue Do spdID:%s, err:%s", url, err)

		return nil, err
	}
	defer resp.Body.Close()

	bodyText, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		log.Get().Errorf("CreateJiraIssue bodyText ReadAll spdID:%s, err:%s", url, err)

		return nil, err
	}

	if resp.StatusCode != http.StatusCreated {
		err := fmt.Sprintf("CreateJiraIssue StatusCode :%d, body:%s", resp.StatusCode, string(bodyText))
		log.Get().Error(err)

		return nil, errors.New(err)
	}

	result := &dao.CreateJiraResult{}
	err = json.Unmarshal(bodyText, &result)
	if err != nil {
		log.Get().Errorf("CreateJiraIssue CreateJiraResult spdID:%s, err:%s", url, err)

		return nil, err
	}

	return result, nil
}

func UpdateJiraLinkInSDP(sdpReqID, issueKey string) error {
	keys := strings.Split(issueKey, "-")
	if len(keys) != 2 {
		log.Get().Errorf("UpdateJiraLinkInSDP issueKey error:%s", issueKey)
	}
	endpoint := KUrlUpdateSdpRequest + strings.TrimSpace(sdpReqID)
	//issuelink := viper.GetString("Jira.host") + "/projects/PMDC/issues/" + issueKey
	issuelink := fmt.Sprintf(KUrlOfIssueLink, keys[0], issueKey)
	inputData := &dao.SdpInputData{
		Request: dao.SdpRequest{
			UdfFields: dao.CreateUdfFields(issuelink),
		},
	}
	rawInput, _ := json.Marshal(inputData)

	data := url.Values{}
	data.Set("input_data", string(rawInput))

	client := &http.Client{}
	r, err := http.NewRequest("PUT", endpoint, strings.NewReader(data.Encode())) // URL-encoded payload
	if err != nil {
		log.Get().Errorf("UpdateJiraLinkInSDP NewRequest err:%s, url:%s, data:,%s", err, endpoint, data)
		return err
	}
	r.Header = http.Header{
		"authtoken": []string{viper.GetString("SDP.token")},
	}
	r.Header.Add("Content-Type", "application/x-www-form-urlencoded")
	r.Header.Add("Content-Length", strconv.Itoa(len(data.Encode())))

	resp, err := client.Do(r)
	if err != nil {
		log.Get().Errorf("UpdateJiraLinkInSDP Do err:%s, url:%s, data:,%s", err, endpoint, data)
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		bodyText, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			log.Get().Errorf("CreateJiraIssue bodyText ReadAll spdID:%s, err:%s", endpoint, err)

			return err
		}
		log.Get().Errorf("CreateJiraIssue StatusCode :%d, body:%s", resp.StatusCode, string(bodyText))

		return errors.New("CreateJiraIssue resp.StatusCode != http.StatusCreated")
	}

	return nil
}
