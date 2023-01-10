/*
 * Copyright © 2021 Dio♦
 * All rights reserved.
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file.
 * ======
 * Created At:    Tuesday, 25th January 2022 9:24:54 am
 * Last Modified: Sunday, 17th April 2022 7:09:45 pm
 * ======
 */

package server

import (
	"evn/lib/utils"
	"evn/server/controller"

	"github.com/gofiber/fiber/v2"
)

type Route struct {
	Ctrl *controller.Controller
}

func New() *Route {
	return &Route{
		Ctrl: controller.New(),
	}
}

func (r *Route) SetupRoutes(app *fiber.App) {

	app.Get("/ping", r.HealthCheck)

	// jira to sdp
	app.Post("jira/webhook", r.Ctrl.JiraWebHook)

	// sdp to jira
	app.Post("sdp/webhook", r.Ctrl.SdpCreateJiraIssue)

	// sdp reopen issue
	app.Post("sdp/reopen-jira", r.Ctrl.SdpReopenIssue)
}

func (r *Route) HealthCheck(ctx *fiber.Ctx) error {
	return utils.WriteSuccessEmptyContent(ctx)
}
