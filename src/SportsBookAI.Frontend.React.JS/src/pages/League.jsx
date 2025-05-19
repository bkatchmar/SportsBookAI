import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom';
import axios from 'axios'
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import BaseAggregationTable from './components/BaseAggregationTable'
import TeamOverUnderTable from './components/TeamOverUnderTable'
import TeamPointSpreadsTable from './components/TeamPointSpreadsTable'
import MatchSelectorAccordian from './components/MatchSelectorAccordian'

function League() {
    const API_URL = import.meta.env.VITE_API_URL
    const { leagueName } = useParams()
    const [aggregatorData, setAggregatorData] = useState({})
    const [matchesThatNeedPredictions, setMatchesThatNeedPredictions] = useState([])

    // Similar to componentDidMount and componentDidUpdate:
    useEffect(() => {
        const fetchData = async () => {
            try {
                const [aggRes, matchesRes] = await Promise.all([
                    axios.get(`${API_URL}/Aggregator/${leagueName}`),
                    axios.get(`${API_URL}/Teams/${leagueName}/matchesThatNeedPredictions`)
                ]);
                setAggregatorData(aggRes.data)
                setMatchesThatNeedPredictions(matchesRes.data)
            } catch (error) {
                console.error('Error fetching data:', error)
            }
        }

        if (Object.keys(aggregatorData).length === 0) {
            fetchData()
        }
    }, [])

    return <Container fluid>
        <Row>
            <Col className="text-center">
                <h1>Welcome to the league data for: {leagueName}</h1>
                <BaseAggregationTable aggregatorData={aggregatorData} />
                <TeamOverUnderTable aggregatorData={aggregatorData} />
                <TeamPointSpreadsTable aggregatorData={aggregatorData} />
            </Col>
        </Row>
        <Row>
            <Col>
                <MatchSelectorAccordian matches={matchesThatNeedPredictions} leagueName={leagueName} />
            </Col>
        </Row>
    </Container>
}

export default League