package dao

import (
	sqlServer "evn/lib/sql_server"
)

const table = `integrate_jira1`

func GetSDPByJira(jiraID string) (*SDPAndJiraMaping, error) {
	query := `
	SELECT jira_id, sdp_id 
	FROM ` + table + ` 
	WHERE jira_id LIKE @p1;`

	maping := &SDPAndJiraMaping{}
	err := sqlServer.SQLsv().Get(maping, query, jiraID)
	if err != nil {
		return nil, err
	}
	return maping, nil
}

func GetJiraBySDP(sdpID string) (*SDPAndJiraMaping, error) {
	query := `
	SELECT jira_id, sdp_id 
	FROM ` + table + ` 
	WHERE sdp_id LIKE @p1;`

	maping := &SDPAndJiraMaping{}
	err := sqlServer.SQLsv().Get(maping, query, sdpID)
	if err != nil {
		return nil, err
	}
	return maping, nil
}

func Insert(maping *SDPAndJiraMaping) error {
	query := `
	INSERT INTO ` + table + ` (jira_id, sdp_id) 
	VALUES ( @p1, @p2);`

	_, err := sqlServer.SQLsv().Exec(query, maping.JiraID, maping.SdpID)
	if err != nil {
		return err
	}
	return nil
}

func UpdateSDP(jiraID, sdpID string) error {
	query := `
	UPDATE ` + table + `
	SET sdp_id=@p2
	WHERE jira_id=@p1;`

	_, err := sqlServer.SQLsv().Exec(query, jiraID, sdpID)
	if err != nil {
		return err
	}
	return nil
}

func UpdateJira(sdpID, jiraID string) error {
	query := `
	UPDATE ` + table + `
	SET jira_id=@p2
	WHERE sdp_id=@p1;`

	_, err := sqlServer.SQLsv().Exec(query, sdpID, jiraID)
	if err != nil {
		return err
	}
	return nil
}
