import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom';
import axios from 'axios'
import Breadcrumb from 'react-bootstrap/Breadcrumb'
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import BaseAggregationTable from './components/BaseAggregationTable'
import TeamOverUnderTable from './components/TeamOverUnderTable'
import TeamPointSpreadsTable from './components/TeamPointSpreadsTable'
import MatchSelectorAccordian from './components/MatchSelectorAccordian'
import { DoesThisLeagueUseWeeks, DoesThisLeagueUsePitchers } from '../utilities/LeagueConstants'

function LeagueTeamData() {
    const API_URL = import.meta.env.VITE_API_URL
    const { leagueName, teamName } = useParams()
    const makeStringNormalCase = (teamString) => teamString.split('-').map(word => word.charAt(0).toUpperCase() + word.slice(1)).join(' ')

    // Similar to componentDidMount and componentDidUpdate:
    useEffect(() => {
    }, [])

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
    </Container>
}

export default LeagueTeamData