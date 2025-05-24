import React, { useState, useEffect } from 'react'
import Table from 'react-bootstrap/Table'
import OverUnderByWeekTable from './OverUnderByWeekTable'

function TeamOverUnderTable(props) {
    const [aggregatorData, setAggregatorData] = useState({})

    useEffect(() => {
        setAggregatorData(props.aggregatorData)
    }, [props.aggregatorData])

    if (Object.keys(aggregatorData).length === 0) {
        return null
    }

    const teamNames = Object.keys(aggregatorData["oversByTeam"])
    return <>
        <h2>Overs and Unders By Team</h2>
        <Table responsive striped hover className="mb-5">
            <thead>
                <tr>
                    <th>Team Name</th>
                    <th>Overs</th>
                    <th>Unders</th>
                </tr>
            </thead>
            <tbody>
                {teamNames.map((name, index) => (
                    <tr key={`league-overunder-card-${index}`}>
                        <td>{name}</td>
                        <td>{aggregatorData["oversByTeam"][name] ?? 0}</td>
                        <td>{aggregatorData["undersByTeam"][name] ?? 0}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
        <OverUnderByWeekTable aggregatorData={aggregatorData} />
    </>
}

export default TeamOverUnderTable