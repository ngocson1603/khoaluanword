package controller

import (
	"evn/server/constant"

	"github.com/spf13/viper"
)

type Controller struct{}

func New() *Controller {
	KUrlCreateJiraIssue = viper.GetString("Jira.host") + "/rest/api/" + viper.GetString("Jira.version") + "/issue"
	KUrlReopenJiraIssue = viper.GetString("Jira.host") + "/rest/api/" + viper.GetString("Jira.version") + "/issue/%s/transitions?expand=transitions.fields" //%s issue ID or Key

	KUrlGetSdpRequest = viper.GetString("SDP.host") + "/api/v3/requests/"
	KUrlUpdateSdpRequest = viper.GetString("SDP.host") + "/api/v3/requests/"

	KUrlOfSdpRequest = viper.GetString("SDP.host") + "/WorkOrder.do?woMode=viewWO&woID="
	KUrlOfIssueLink = viper.GetString("Jira.host") + "/projects/%s/issues/%s" // %s%s: Project Key, Issue Key

	constant.KProjects = viper.GetStringMapString("Projects")

	return &Controller{}
}

const (
	KDefaultProjectKey string = "PMDC"
	KJiraIssueType     string = "Service desk - Jira"
	KClosed            string = "Closed"
)

var (
	KUrlCreateJiraIssue string
	KUrlReopenJiraIssue string

	KUrlGetSdpRequest    string
	KUrlUpdateSdpRequest string

	KUrlOfSdpRequest string
	KUrlOfIssueLink  string
)
