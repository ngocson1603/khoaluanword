/*
 * Copyright © 2021 Dio♦
 * All rights reserved.
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file.
 * ======
 * Created At:    Monday, 24th January 2022 10:59:26 pm
 * Last Modified: Thursday, 17th November 2022 8:13:13 pm
 * ======
 */

package main

import (
	"errors"
	"fmt"
	"net/http"
	"os"
	"strings"

	log "evn/lib/logger"
	sqlServer "evn/lib/sql_server"
	"evn/lib/winapi"
	"evn/server"

	"github.com/gofiber/fiber/v2"
	"github.com/gofiber/fiber/v2/middleware/cors"
	"github.com/gofiber/fiber/v2/middleware/logger"
	fibRecover "github.com/gofiber/fiber/v2/middleware/recover"
	"github.com/spf13/viper"
)

func init() {
	if _, err := os.Stat("config.json"); errors.Is(err, os.ErrNotExist) {
		winapi.MessageBoxPlain("error", err.Error())
		panic(err)
	}

	viper.SetConfigFile("config.json")
	err := viper.ReadInConfig()
	if err != nil {
		winapi.MessageBoxPlain("error", err.Error())
		panic(err)
	}
	if viper.GetBool("Debug") {
		fmt.Println("====== RUN on DEBUG mode =======")
	} else {
		fmt.Println("====== RUN on PRODUCTION =======")
	}
}

// MAIN
func main() {
	defer func() {
		if err := recover(); err != nil {
			log.Get().Infof("App Panic err: %v", err)
			panic(err)
		}
	}()

	checkConnection(viper.GetString("Jira.host"))
	checkConnection(viper.GetString("SDP.host"))

	log.Install()
	sqlServer.Initialize()

	app := fiber.New()
	app.Use(cors.New())
	app.Use(fibRecover.New(fibRecover.Config{EnableStackTrace: true}))

	if viper.GetBool("Debug") {
		app.Use(logger.New(logger.Config{Next: func(c *fiber.Ctx) bool {
			return strings.Contains(c.Request().URI().String(), "healthcheck")
		}}))
	}

	r := server.New()
	r.SetupRoutes(app)

	// Start application
	err := app.Listen(viper.GetString("Server.port"))
	if err != nil {
		log.Get().Errorf("app.Listen err: %v", err)
		panic(err)
	}
}

func checkConnection(url string) {
	_, err := http.Get(url)
	if err != nil {
		code := winapi.MessageBoxPlain("error", err.Error())
		os.Exit(code)
	}
}
