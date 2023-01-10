/*
 * Copyright © 2021 Dio♦
 * All rights reserved.
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file.
 * ======
 * Created At:    Wednesday, 26th January 2022 12:41:39 am
 * Last Modified: Tuesday, 6th September 2022 8:35:02 am
 * ======
 */

package dao

type SDPAndJiraMaping struct {
	SdpID  string `db:"sdp_id"`
	JiraID string `db:"jira_id"`
}

/* Webhook */
type JiraWebhookData struct {
	Issue Issue `json:"issue"`
}

type Issue struct {
	ID     string `json:"id"`
	Self   string `json:"self"`
	Key    string `json:"key"`
	Fields Fields `json:"fields"`
}

type Fields struct {
	Project          *Project    `json:"project"` // project key - category
	Issuetype        Issuetype   `json:"issuetype"`
	Status           Status      `json:"status"`
	Resolution       *Resolution `json:"resolution,omitempty"`
	Customfield10018 string      `json:"customfield_10018"`           // nguyên nhân
	Customfield10019 string      `json:"customfield_10019"`           // cách giải quyết
	Customfield17860 *CFValue    `json:"customfield_17860,omitempty"` // Giai đoạn phát sinh issue - udf_pick_2713
	Customfield16760 *CFValue    `json:"customfield_16760,omitempty"` // Trạng thái theo 5249 - udf_pick_2712
	Customfield16562 *CFValue    `json:"customfield_16562,omitempty"` // Issue trùng lặp - udf_pick_2714

	Customfield11761 *CFValue `json:"customfield_11761,omitempty"` // Phân loại chi tiết yêu cầu - Phân loại chi tiết
	Customfield13164 *CFValue `json:"customfield_13164,omitempty"` // Cập nhật đặc tả
	Customfield11961 *CFValue `json:"customfield_11961,omitempty"` // Cập nhật source code
	Customfield14860 *CFValue `json:"customfield_14860,omitempty"` // Cập nhật dữ liệu
}

type Status struct {
	Self string `json:"self"`
	Name string `json:"name"`
	ID   string `json:"id"`
}

type CFValue struct {
	Self  string `json:"self"`
	Value string `json:"value"`
	ID    string `json:"id"`
}

type Issuetype struct {
	Self     string `json:"self"`
	ID       string `json:"id"`
	Name     string `json:"name"`
	Subtask  bool   `json:"subtask"`
	AvatarID int64  `json:"avatarId"`
}

type Resolution struct {
	Self        string `json:"self"`
	ID          string `json:"id"`
	Description string `json:"description"`
	Name        string `json:"name"`
}

func CreateSDPCustomfield(field *CFValue) string {
	if field != nil {
		return field.Value
	}
	return ""
}
