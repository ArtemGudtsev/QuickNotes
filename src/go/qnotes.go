package main

import (
	"fmt"
	"os"
	"path"
	"regexp"
	"strings"
	"time"
)

func main() {
	const folderWithNotes = "E:\\out\\qnotes" //todo - https://github.com/ArtemGudtsev/QuickNotes/projects/1#card-37631245
	const msgPrefixError = "ERROR"
	const msgPrefixHelp = "HELP"
	const notesFileExtension = ".qnote"

	var notesFileName = time.Now().Format("20060102") + notesFileExtension
	var notesFilePath = path.Join(folderWithNotes, notesFileName)
	var tags []string

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

		fmt.Printf("[%s][%s] ", timePrefix, tagsSuffix)

		var _, scanError = fmt.Scan(&record)// todo - is err processing required here?

		if scanError != nil {
			panic(scanError)
		}

		strings.Trim(record, " ")

		if record == "" {
			fmt.Printf("[%s] you can't save empty record to notes!\n", msgPrefixError)
			continue
		}

		if paramHunter.MatchString(record) {
			if paramHelpHunter.MatchString(record) {

			} else if paramExitHunter.MatchString(record) {
				return
			} else if paramAddTagHunter.MatchString(record) {

			} else if paramClearTagsHunter.MatchString(record) {

			} else {

			}
		} else {
			var resultRecord = fmt.Sprintf("[%s][%s] %s", timePrefix, tagsSuffix, record)
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

	for i := range tags {
		result += tags[i]// todo - https://github.com/ArtemGudtsev/QuickNotes/projects/1#card-37643704
	}

	return result
}

