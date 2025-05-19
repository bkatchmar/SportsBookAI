import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom';
import axios from 'axios'
import Skeleton from 'react-loading-skeleton'
import Accordion from 'react-bootstrap/Accordion'
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import Table from 'react-bootstrap/Table'
import Form from 'react-bootstrap/Form'

function League() {
    const API_URL = import.meta.env.VITE_API_URL
    const displayPercent = (percent) => (percent) ? `${(percent * 100).toFixed(2)}%` : '0%'
    const MINUS = "MINUS", PLUS = "PLUS"
    const { leagueName } = useParams()
    const [aggregatorData, setAggregatorData] = useState({})
    const [matchesThatNeedPredictions, setMatchesThatNeedPredictions] = useState([])
    const [selectedMatch, setSelectedMatch] = useState('')

    // Similar to componentDidMount and componentDidUpdate:
    useEffect(() => {
        const fetchData = async () => {
            try {
                const aggregatorResult = await axios.get(`${API_URL}/Aggregator/${leagueName}`)
                const matchesResult = await axios.get(`${API_URL}/Teams/${leagueName}/matchesThatNeedPredictions`)
                setAggregatorData(aggregatorResult["data"])
                setMatchesThatNeedPredictions(matchesResult["data"])
            } catch (error) {
                console.error('Error fetching data:', error)
            }
        }

        if (Object.keys(aggregatorData).length === 0) {
            fetchData()
        }
    }, [])

    function returnBaseSkeletonOrBaseAggregationData(data) {
        if (Object.keys(aggregatorData).length === 0) {
            return <Skeleton count={10} />
        } else {
            return <Table responsive striped hover className="mb-5">
                <thead>
                    <tr>
                        <th>Over %</th>
                        <th>Under %</th>
                        <th>Spread Cover %</th>
                        <th>Prevent Spread Cover %</th>
                    </tr>
                    <tr>
                        <td>{displayPercent(data["allOverPercentage"])}</td>
                        <td>{displayPercent(data["allUnderPercentage"])}</td>
                        <td>{displayPercent(data["allMinusSpreadsPercentage"])}</td>
                        <td>{displayPercent(data["allPlusSpreadsPercentage"])}</td>
                    </tr>
                </thead>
            </Table>
        }
    }

    function returnOversAndUndersByTeamTable(data) {
        if (Object.keys(aggregatorData).length === 0) {
            return null
        } else {
            const teamNames = Object.keys(aggregatorData["oversByTeam"])
            return <Table responsive striped hover className="mb-5">
                <thead>
                    <tr>
                        <th>Team Name</th>
                        <th>Overs</th>
                        <th>Unders</th>
                    </tr>
                    {teamNames.map((name, index) => (
                        <tr key={`league-overunder-card-${index}`}>
                            <td>{name}</td>
                            <td>{data["oversByTeam"][name] ?? 0}</td>
                            <td>{data["undersByTeam"][name] ?? 0}</td>
                        </tr>
                    ))}
                </thead>
            </Table>
        }
    }

    function returnPointSpreadsByTeamTable(data) {
        if (!aggregatorData.hasOwnProperty("pointSpreadRecords")) {
            return null
        } else {
            const teamNames = Object.keys(aggregatorData["pointSpreadRecords"])
            return <Table responsive striped hover className="mb-5">
                <thead>
                    <tr>
                        <th>Team Name</th>
                        <th>Minus Side</th>
                        <th>Plus Side</th>
                    </tr>
                    {teamNames.map((name, index) => (
                        <tr key={`league=pointspread-card-${index}`}>
                            <td>{name}</td>
                            <td>{returnPointSpreadRecord(data, name, MINUS)}</td>
                            <td>{returnPointSpreadRecord(data, name, PLUS)}</td>
                        </tr>
                    ))}
                </thead>
            </Table>
        }
    }

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

    async function getPredictions() {
        const getPrdictions = await axios.post(`${API_URL}/Aggregator/getPredictions`, {
            LeagueName: leagueName,
            MatchId: ''
        })
    }

    function renderSimulationAccordian(matches) {
        if (matches.length == 0) {
            return null
        } else {
            return <Accordion>
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Simulate Matchup</Accordion.Header>
                    <Accordion.Body>
                        <Form>
                            <Form.Group className="mb-1" controlId="matchup.Match">
                                <Form.Label>Match</Form.Label>
                                <Form.Select aria-label="Pick Match" value={selectedMatch} onChange={(e) => {
                                    setSelectedMatch(e.target.value);
                                }}>
                                    <option value="">Pick A Match</option>
                                    {matches.map((match, index) => (
                                        <option key={`home-team-option-${index}`} value={match["id"]}>{match["display"]}</option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Form>
                    </Accordion.Body>
                </Accordion.Item>
            </Accordion>
        }
    }

    return <Container fluid>
        <Row>
            <Col className="text-center">
                <h1>Welcome to the league data for: {leagueName}</h1>
                {returnBaseSkeletonOrBaseAggregationData(aggregatorData)}
                {Object.keys(aggregatorData).length === 0 ? null : <h2>Overs and Unders By Team</h2>}
                {returnOversAndUndersByTeamTable(aggregatorData)}
                {Object.keys(aggregatorData).length === 0 ? null : <h2>Point Spread Records By Team</h2>}
                {returnPointSpreadsByTeamTable(aggregatorData)}
            </Col>
        </Row>
        <Row>
            {renderSimulationAccordian(matchesThatNeedPredictions)}
        </Row>
    </Container>
}

export default League;