import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import Table from 'react-bootstrap/Table'
import OverUnderByWeekTable from './OverUnderByWeekTable'

function TeamOverUnderTable(props) {
    const [aggregatorData, setAggregatorData] = useState({})
    const [leagueName, setLeagueName] = useState('')
    const makeTeamNameSlug = (teamString) => teamString.toLowerCase().replace(/\s+/g, '-')

    useEffect(() => {
        setAggregatorData(props.aggregatorData)
    }, [props.aggregatorData])

    useEffect(() => {
        if (props.leagueName !== leagueName) {
            setLeagueName(props.leagueName)
        }
    }, [props.leagueName])

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
                        <td>
                            <Link to={`/${leagueName}/${makeTeamNameSlug(name)}`}>{name}</Link>
                        </td>
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