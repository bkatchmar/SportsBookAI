import React, { useState, useEffect } from 'react'
import Table from 'react-bootstrap/Table'

function TeamPointSpreadsTable(props) {
    const MINUS = "MINUS", PLUS = "PLUS"
    const [aggregatorData, setAggregatorData] = useState({})

    useEffect(() => {
        setAggregatorData(props.aggregatorData)
    }, [props.aggregatorData])

    function returnPointSpreadRecord(data, teamName, side) {
        if (data["pointSpreadRecords"] && data["pointSpreadRecords"][teamName]) {
            const teamRecord = data["pointSpreadRecords"][teamName]
            for (let x = 0; x < teamRecord.length; x++) {
                let element = teamRecord[x]
                if (element["side"] === side) return `${element["wins"]}-${element["losses"]}`
            }
        }

        return "0-0"
    }

    if (!aggregatorData.hasOwnProperty("pointSpreadRecords")) {
        return null
    }

    const teamNames = Object.keys(aggregatorData["pointSpreadRecords"]).sort()
    return <>
        <h2>Point Spread Records By Team</h2>
        <Table responsive striped hover className="mb-5">
            <thead>
                <tr>
                    <th>Team Name</th>
                    <th>Minus Side</th>
                    <th>Plus Side</th>
                </tr>
            </thead>
            <tbody>
                {teamNames.map((name, index) => (
                    <tr key={`league=pointspread-card-${index}`}>
                        <td>{name}</td>
                        <td>{returnPointSpreadRecord(aggregatorData, name, MINUS)}</td>
                        <td>{returnPointSpreadRecord(aggregatorData, name, PLUS)}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    </>
}

export default TeamPointSpreadsTable