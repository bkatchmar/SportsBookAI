import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom';
import axios from 'axios'
import Breadcrumb from 'react-bootstrap/Breadcrumb'
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import BaseAggregationTable from './components/BaseAggregationTable'
import SecondaryAggregationTable from './components/SecondaryAggregationTable'
import TeamOverUnderTable from './components/TeamOverUnderTable'
import TeamPointSpreadsTable from './components/TeamPointSpreadsTable'

function LeagueTeamData() {
    const API_URL = import.meta.env.VITE_API_URL
    const { leagueName, teamName } = useParams()
    const makeStringNormalCase = (teamString) => teamString.split('-').map(word => word.charAt(0).toUpperCase() + word.slice(1)).join(' ')
    const [aggregatorData, setAggregatorData] = useState({})
    const [currentTeam, setCurrentTeam] = useState('')

    const fetchData = async () => {
        try {
            const aggRes = await axios.get(`${API_URL}/Aggregator/${leagueName}/${teamName}`)
            setCurrentTeam(teamName)
            setAggregatorData(aggRes["data"])
        } catch (error) {
            console.error('Error fetching data:', error)
        }
    }

    // Similar to componentDidMount and componentDidUpdate:
    useEffect(() => {
        if (Object.keys(aggregatorData).length === 0) {
            fetchData()
        }
    }, [])
    useEffect(() => {
        if (Object.keys(aggregatorData).length === 0 || currentTeam !== teamName) {
            setAggregatorData({})
            fetchData()
        }
    }, [teamName])

    return <Container fluid>
        <Breadcrumb>
            <Breadcrumb.Item href="/">Home</Breadcrumb.Item>
            <Breadcrumb.Item href={`/${leagueName}`}>
                {leagueName} Total Data
            </Breadcrumb.Item>
            <Breadcrumb.Item href="#">
                {makeStringNormalCase(teamName)}
            </Breadcrumb.Item>
        </Breadcrumb>
        <Row>
            <Col className="text-center">
                <h1>Welcome to the league data for the team: {makeStringNormalCase(teamName)}</h1>
                <BaseAggregationTable aggregatorData={aggregatorData} />
                <SecondaryAggregationTable aggregatorData={aggregatorData} />
                <TeamOverUnderTable aggregatorData={aggregatorData} leagueName={leagueName} />
                <TeamPointSpreadsTable aggregatorData={aggregatorData} />
            </Col>
        </Row>
    </Container>
}

export default LeagueTeamData