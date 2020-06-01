package main

import (
	"fmt"
	"os"
	"path"
	"regexp"
	"strings"
	"time"
)

const msgPrefixError = "ERROR"
const msgPrefixHelp = "HELP"

func main() {
	//todo - https://github.com/ArtemGudtsev/QuickNotes/projects/1#card-37631245
	const folderWithNotes = "E:\\out\\qnotes"
	const notesFileExtension = ".qnote"

	var notesFileName = time.Now().Format("20060102") + notesFileExtension
	var notesFilePath = path.Join(folderWithNotes, notesFileName)
	var tags []string

	_ = os.Mkdir(folderWithNotes, 0660)// attempt to create folder and ignore errors

	var notesFile, notesFileError = os.OpenFile(notesFilePath, os.O_CREATE|os.O_APPEND, 0660)

	if notesFileError != nil {
			panic(notesFileError)
	}

	defer func() {
		var closeFileError = notesFile.Close()

		if closeFileError != nil {
			panic(closeFileError)
		}
	}()

	// todo - https://github.com/ArtemGudtsev/QuickNotes/projects/1#card-37643784
	var paramHunter = regexp.MustCompile("^(-|--)")
	var paramHelpHunter = regexp.MustCompile("^(-h|--help)")
	var paramExitHunter = regexp.MustCompile("^(-x|--exit)")
	var paramAddTagHunter = regexp.MustCompile("^(--add-tag)")
	var paramClearTagsHunter = regexp.MustCompile("^(--clear-tags)")
	
	for true {
		var timePrefix = time.Now().Format("15:04:05")
		var tagsSuffix = getTagsCollection(tags)
		var record string

		//fmt.Printf("[%s]%s ", timePrefix, tagsSuffix)
		fmt.Printf("[%s] ", timePrefix)

		//var _, scanError = fmt.Scanln(&record)
		var _, scanError = fmt.Scan(&record)

		if scanError != nil {
			panic(scanError)
		}

		strings.Trim(record, " ")

		if record == "" || record == "\n" {
			fmt.Printf("[%s] you can't save empty record to notes!\n", msgPrefixError)
			continue
		}

		if paramHunter.MatchString(record) {
			if paramHelpHunter.MatchString(record) {
				showHelp()
			} else if paramExitHunter.MatchString(record) {
				return
			} else if paramAddTagHunter.MatchString(record) {
				var index = len("--add-tag")
				var tag = ""

				if index < len(record) {
					tag = string(record[:index])
				}

				if tag != "" {
					tags = append(tags, tag)
				} else {
					showErrorMsg("provided tag name was empty!")
				}
			} else if paramClearTagsHunter.MatchString(record) {
				tags = tags[:0]
			} else {
				showErrorMsg(fmt.Sprintf("Parameter %s can't be recognized", record))
			}
		} else {
			var resultRecord = fmt.Sprintf("[%s]%s %s", timePrefix, tagsSuffix, record)
			// todo - https://github.com/ArtemGudtsev/QuickNotes/projects/1#card-37643592
			var _, notesFileWriteError = notesFile.WriteString(resultRecord + "\n")

			if notesFileWriteError != nil {
				panic(notesFileWriteError)
			}
		}
	}
}

func getTagsCollection(tags []string) string {
	var result = ""

	if tags == nil || len(tags) == 0 {
		return result
	}

	result = "["
	for i := range tags {
		// todo - https://github.com/ArtemGudtsev/QuickNotes/projects/1#card-37643704
		result += tags[i]
	}
	result += "]"

	return result
}

func showHelp() {
	showHelpMsg("-h (--help) - will show this help")
	showHelpMsg("-x (--exit) - will close tool")
	showHelpMsg("--add-tag=<tag-name> - will add tag to tags list for next records")
	showHelpMsg("--clear-tags - will clean tags list")
}

func showHelpMsg(msg string) {
	fmt.Printf("[%s] %s\n", msgPrefixHelp, msg)
}

func showErrorMsg(msg string)  {
	fmt.Printf("[%s] %s\n", msgPrefixError, msg)
}

