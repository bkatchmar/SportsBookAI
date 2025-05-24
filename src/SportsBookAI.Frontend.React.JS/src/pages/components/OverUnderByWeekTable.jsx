import React, { useState, useEffect } from 'react'
import Table from 'react-bootstrap/Table'

function OverUnderByWeekTable(props) {
    const [aggregatorData, setAggregatorData] = useState({})

    useEffect(() => {
        setAggregatorData(props.aggregatorData)
    }, [props.aggregatorData])

    if (Object.keys(aggregatorData).length === 0 || !aggregatorData["allWeekRecords"]) {
        return null
    }
    const weekRecords = Object.keys(aggregatorData["allWeekRecords"])
    return <>
        <h2>Overs and Unders Records By Week</h2>
        <Table responsive striped hover className="mb-5">
            <thead>
                <tr>
                    <th>Week Number</th>
                    <th>Overs</th>
                    <th>Unders</th>
                </tr>
            </thead>
            <tbody>
                {aggregatorData["allWeekRecords"].map((week, index) => (
                    <tr key={`league-week-overunder-card-${index}`}>
                        <td>{week["week"]}</td>
                        <td>{week["overs"]}</td>
                        <td>{week["unders"]}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    </>
}

export default OverUnderByWeekTable