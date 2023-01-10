package log

import (
	"fmt"
	"os"
	"time"

	"github.com/spf13/viper"
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"gopkg.in/natefinch/lumberjack.v2"
)

var (
	logInstance *zap.SugaredLogger
)

func Get() *zap.SugaredLogger {
	if logInstance == nil {
		Install()
	}
	return logInstance
}

func Install() {
	conf := getConfigFromEnv()
	if _, err := os.Stat(conf.Path); os.IsNotExist(err) {
		err := os.Mkdir(conf.Path, os.ModeDir)
		if err != nil {
			panic(err)
		}
	}

	logRotation := &lumberjack.Logger{
		Filename:   conf.Path + conf.FileName,
		MaxSize:    50, // megabytes
		MaxBackups: 60, // files
		MaxAge:     30, // days
		LocalTime:  true,
	}

	encoder := getEncoder()

	var w zapcore.WriteSyncer
	if viper.GetBool("Debug") {
		w = zapcore.NewMultiWriteSyncer(
			zapcore.AddSync(os.Stdout),
			zapcore.AddSync(logRotation),
		)
	} else {
		w = zapcore.NewMultiWriteSyncer(
			zapcore.AddSync(logRotation),
		)
	}

	core := zapcore.NewCore(
		encoder,
		w,
		zapcore.DebugLevel,
	)

	logInstance = zap.New(core, zap.AddCaller()).Sugar()
	fmt.Println("====> Log Install Done!")
}

func getEncoder() zapcore.Encoder {
	return zapcore.NewConsoleEncoder(zapcore.EncoderConfig{
		MessageKey:       "msg",
		TimeKey:          "time",
		LevelKey:         "level",
		CallerKey:        "caller",
		EncodeLevel:      CustomLevelEncoder,         //Format cách hiển thị level log
		EncodeTime:       SyslogTimeEncoder,          //Format hiển thị thời điểm log
		EncodeCaller:     zapcore.ShortCallerEncoder, //Format caller
		ConsoleSeparator: " | ",
	})
}

func SyslogTimeEncoder(t time.Time, enc zapcore.PrimitiveArrayEncoder) {
	enc.AppendString(t.Format("2006-01-02 15:04:05"))
}

func CustomLevelEncoder(level zapcore.Level, enc zapcore.PrimitiveArrayEncoder) {
	enc.AppendString(level.CapitalString())
}
