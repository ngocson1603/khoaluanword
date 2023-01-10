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
	"net/http"
	"strings"

	"github.com/gofiber/fiber/v2"
	"github.com/spf13/viper"
)

func (ctrl *Controller) SdpReopenIssue(ctx *fiber.Ctx) error {
	requestID := ctx.Query("req_id", "")

	log.Get().Infof("SdpReopenIssue =====> Start: reqID [%s] ", requestID)

	if requestID == "" {
		log.Get().Errorf("SdpReopenIssue validate requestID:%s", requestID)

		return utils.WriteError(ctx, fiber.StatusBadRequest, errors.New("invalid request id"))
	}

	reqID := strings.TrimSpace(requestID)

	// get Jira ID by SDP ID from DB
	maping, err := dao.GetJiraBySDP(reqID)
	if err != nil {
		log.Get().Errorf("SdpReopenIssue GetJiraBySDP err:%v", err)
	}
	if maping == nil {
		return utils.WriteError(ctx, fiber.StatusBadRequest, errors.New("issue chua duoc tao tren jira"))
	}

	// todo: check status SDP != Resolve hoặc Close thì mới reopen jira issue

	err = reopenJiraIssue(maping.JiraID)
	if err != nil {
		log.Get().Errorf("SdpReopenIssue reopenJiraIssue err:%v", err)
		return utils.WriteError(ctx, fiber.StatusInternalServerError, err)
	}

	log.Get().Infof("SdpReopenIssue =====> With no error Done: [%s] ", reqID)

	// Return
	return utils.WriteSuccessEmptyContent(ctx)
}

type Payload struct {
	Fields     *Fields    `json:"fields,omitempty"`
	Transition Transition `json:"transition"`
}

type Fields struct {
	Customfield12761 string `json:"customfield_12761,omitempty"`
}

type Transition struct {
	ID string `json:"id"`
}

func reopenJiraIssue(issueID string) error {
	// Url
	url := fmt.Sprintf(KUrlReopenJiraIssue, issueID)

	data := Payload{
		Transition: Transition{ID: "3"}, // 3: Reopen
	}
	payloadBytes, err := json.Marshal(data)
	if err != nil {
		return err
	}
	body := bytes.NewReader(payloadBytes)

	req, err := http.NewRequest("POST", url, body)
	if err != nil {
		return err
	}
	req.SetBasicAuth(viper.GetString("Jira.username"), viper.GetString("Jira.password"))
	req.Header.Set("Accept", "application/json")
	req.Header.Set("Content-Type", "application/json")

	resp, err := http.DefaultClient.Do(req)

	if err != nil {
		if resp.StatusCode == fiber.StatusBadRequest && strings.Contains(string(err.Error()), constant.KLichSuKhongDatYC) {
			data := Payload{
				Fields: &Fields{
					Customfield12761: constant.KLichSuKhongDatYCDafault,
				},
				Transition: Transition{ID: "3"}, // 3: Reopen
			}
			payloadBytes, err = json.Marshal(data)
			if err != nil {
				return err
			}
			body := bytes.NewReader(payloadBytes)

			req, err = http.NewRequest("POST", url, body)
			if err != nil {
				return err
			}
			req.SetBasicAuth(viper.GetString("Jira.username"), viper.GetString("Jira.password"))
			req.Header.Set("Accept", "application/json")
			req.Header.Set("Content-Type", "application/json")

			resp, err = http.DefaultClient.Do(req)
		}

		if err != nil {
			return err
		}
	}
	defer resp.Body.Close()

	if resp.StatusCode != fiber.StatusNoContent {
		bodyBytes, _ := io.ReadAll(resp.Body)
		err := fmt.Errorf("close issue failed with status:%d, body:%v", resp.StatusCode, string(bodyBytes))
		return err
	}

	return nil
}
