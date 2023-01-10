package dao

type SdpInputData struct {
	Request SdpRequest `json:"request"`
}

type SdpRequest struct {
	Resolution *SdpResolution `json:"resolution,omitempty"`
	Status     *SdpStatus     `json:"status,omitempty"`
	UdfFields  *SdpUdfFields  `json:"udf_fields,omitempty"`
}

type SdpResolution struct {
	Content string `json:"content"`
}

type SdpStatus struct {
	Name string `json:"name"`
}

type SdpUdfFields struct {
	UdfSline2710 string `json:"udf_sline_2710,omitempty"`
	// UdfPick2712 string `json:"udf_pick_2712,omitempty"` // Trạng thái theo 5249
	// UdfPick2713 string `json:"udf_pick_2713,omitempty"` // Giai đoạn phát sinh issue
	// UdfPick2714 string `json:"udf_pick_2714,omitempty"` // Issue trùng lặp

	UdfPick2704  string `json:"udf_pick_2704,omitempty"`  // Phân loại chi tiết
	UdfSline3907 string `json:"udf_sline_3907,omitempty"` // Cập nhật đặc tả
	UdfPick3908  string `json:"udf_pick_3908,omitempty"`  // Cập nhật source code
	UdfPick3911  string `json:"udf_pick_3911,omitempty"`  // Cập nhật dữ liệu
	UdfMline4206 string `json:"udf_mline_4206,omitempty"` // Nguyên nhân
	UdfMline4207 string `json:"udf_mline_4207,omitempty"` // Cách giải quyết
}

func CreateResolution(content string) *SdpResolution {
	if len(content) > 0 {
		return &SdpResolution{Content: content}
	}
	return nil
}

func CreateStatus(status string) *SdpStatus {
	if len(status) > 0 {
		return &SdpStatus{Name: status}
	}
	return nil
}

func CreateUdfFields(fields string) *SdpUdfFields {
	if len(fields) > 0 {
		return &SdpUdfFields{UdfSline2710: fields}
	}
	return nil
}
