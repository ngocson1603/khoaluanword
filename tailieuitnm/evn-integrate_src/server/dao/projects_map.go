package dao

// Tên trên SDP : Key trên jira
var ProjectsMap = map[string]string{
	"CMIS - Hệ thống thông tin quản lý khách hàng":               "CMISIII",   // QLKDD: CMIS 3.0
	"PMIS - Hệ thống quản lý kỹ thuật lưới điện":                 "PMIS",      // QLKT: PMIS lưới
	"EAM - Hệ thống quản lý kỹ thuật nguồn":                      "EAM",       // QLKT: PMIS nguồn + Dự toán SCL
	"MDMS - Hệ thống kho dữ liệu đo đếm dùng chung của Tập đoàn": "MDMS",      // QLKT: Kho dữ liệu đo đếm EVN
	"EVNHES - Hệ thống thu thập dữ liệu công tơ điện tử":         "EVNHES",    // QLKT: EVN HES (Hệ thống thu thập dữ liệu đo đếm từ xa)
	"DOFFICE - Hệ thống quản lý văn bản điều hành":               "DOFFICE",   // DOFFICE - Văn phòng số
	"IMIS - Hệ thống quản lý đầu tư xây dựng":                    "DTXDCB",    // Quản lý đầu tư xây dựng
	"HRMS - Hệ thống quản lý nhân sự":                            "HRMS",      // QTDN: HRMS (Quản trị nhân sự)
	"EVNPORAL - Hệ thống Portal của EVN":                         "EVNPORTAL", // QA: EVN Portal
	"ERP - Hệ thống thông tin quản lý nguồn lực doanh nghiệp":    "EBS",       // ERP-GOLIVE: Triển khai, vận hành hệ thống ERP (Test SDP)
	"(Các phần mềm khác trừ ERP)":                                "PMDC",      // Phần mềm dùng chung (Test SDP)
}
