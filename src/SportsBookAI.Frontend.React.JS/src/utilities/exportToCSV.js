import { saveAs } from "file-saver"
import * as XLSX from "xlsx"

export default function exportToCSV(jsonData, fileName = "data.csv") {
    const worksheet = XLSX.utils.json_to_sheet(jsonData)
    const workbook = XLSX.utils.book_new()
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1")

    const csvArrayBuffer = XLSX.write(workbook, {
        bookType: "csv",
        type: "array", // Must be 'array' for front-end usage
    })

    const blob = new Blob([csvArrayBuffer], { type: "text/csv;charset=utf-8;" })
    saveAs(blob, fileName)
}