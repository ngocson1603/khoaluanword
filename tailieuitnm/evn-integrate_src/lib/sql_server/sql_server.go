/*
 * Copyright © 2021 Dio♦
 * All rights reserved.
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file.
 * ======
 * Created At:    Tuesday, 25th January 2022 1:41:12 pm
 * Last Modified: Wednesday, 30th March 2022 8:17:53 am
 * ======
 */
package sql_server

import (
	log "evn/lib/logger"
	"fmt"
	"time"

	_ "github.com/denisenkom/go-mssqldb"
	"github.com/jmoiron/sqlx"
	"github.com/spf13/viper"
)

var conn *sqlx.DB

func SQLsv() *sqlx.DB {
	if conn == nil {
		Initialize()
	}
	return conn
}

func Initialize() {

	var server = viper.GetString("SqlServer.host")
	var port = viper.GetInt("SqlServer.port")
	var user = viper.GetString("SqlServer.user")
	var password = viper.GetString("SqlServer.password")
	var database = viper.GetString("SqlServer.database")
	var active = viper.GetInt("SqlServer.active")
	var idle = viper.GetInt("SqlServer.idle")
	var lifetime = viper.GetInt("SqlServer.lifetime")

	// Build connection string
	connString := fmt.Sprintf("server=%s;user id=%s;password=%s;port=%d;database=%s;",
		server, user, password, port, database)

	fmt.Printf("======> sqlserver input: %s\n", connString)
	var err error

	// Create connection pool
	conn, err = sqlx.Connect("sqlserver", connString)
	if err != nil {
		log.Get().Errorf("Error creating connection pool: %v", err)
		panic(err)
	}

	conn.SetMaxOpenConns(active)
	conn.SetMaxIdleConns(idle)
	if lifetime < 60 {
		lifetime = 5 * 60
	}
	conn.SetConnMaxLifetime(time.Duration(lifetime) * time.Second)

	fmt.Printf("======> sqlserver Connected: %+v\n", conn.Stats())

	// create tables
	err = createTables(conn)
	if err != nil {
		fmt.Printf("======> create integrate_jira err:%v\n", err)
	}
}

func createTables(conn *sqlx.DB) error {
	queryStr := `
	IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='integrate_jira1' and xtype='U')
    CREATE TABLE integrate_jira1 (
		jira_id varchar(20) NOT NULL,
        sdp_id varchar(20) NOT NULL,
		created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
		PRIMARY KEY (jira_id)
    );
	CREATE UNIQUE INDEX sdp_id_uniq_integrate_jira1
	ON integrate_jira1(sdp_id);
	`

	_, err := conn.Exec(queryStr)
	return err
}
