/*
 * Copyright © 2021 Dio♦
 * All rights reserved.
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file.
 * ======
 * Created At:    Wednesday, 26th January 2022 12:29:50 am
 * Last Modified: Sunday, 27th February 2022 4:38:41 pm
 * ======
 */

package jira_client

import (
	"fmt"

	"github.com/andygrunwald/go-jira"
	"github.com/spf13/viper"
)

var JiraClient *jira.Client

func Initialize() {
	base := viper.GetString("Jira.host")
	tp := jira.BasicAuthTransport{
		Username: viper.GetString("Jira.username"),
		Password: viper.GetString("Jira.password"),
	}

	var err error
	JiraClient, err = jira.NewClient(tp.Client(), base)
	if err != nil {
		fmt.Printf("jira client err: %+v\n", err)
		panic(err)
	}
	fmt.Println("======> INIT JIRA client ")
}

func GetInstance() *jira.Client {
	return JiraClient
}
