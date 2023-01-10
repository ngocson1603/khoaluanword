# EVN

## Mô tả

Dự án tích hợp hệ thống Jira-SDP-DOffice

## TODO

- [x] sql server
  + [x] connect
  + [x] create db
- [x] Jira
  + [x] webhooks => update SDP status
- [x] ServiceDesk Plus
  + [x] create jira issues
- [x] DOffice

## Build and Run
- run
```go
go run main.go
```
- build
```go
go build -o .\bin\evn_integrated_v{xxx}.exe main.go
```