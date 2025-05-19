import React, { useState, useEffect } from 'react'
import axios from 'axios'
import Skeleton from 'react-loading-skeleton'
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import Card from 'react-bootstrap/Card'

function Home() {
    const API_URL = import.meta.env.VITE_API_URL
    const [leagues, setLeagues] = useState([])

    // Similar to componentDidMount and componentDidUpdate:
    useEffect(() => {
        const fetchData = async () => {
            try {
                const result = await axios.get(`${API_URL}/Leagues`)
                setLeagues(result["data"])
            } catch (error) {
                console.error('Error fetching data:', error)
            }
        }

        if (leagues.length === 0) {
            fetchData()
        }
    }, [])

    function returnBaseSkeletonOrCards(leaguesFromState) {
        if (leaguesFromState.length === 0) {
            return <Skeleton count={5} />
        } else {
            return leaguesFromState.map((league, index) => (
                <Card key={`league-card-${index}`} className="mt-5 mb-5">
                    <Card.Title>{league}</Card.Title>
                    <Card.Link href={`/${league}`}>Go To League Aggregator</Card.Link>
                </Card>
            ))
        }
    }

    return <Container fluid>
        <Row>
            <Col className="text-center">
                <h1>Welcome to SportsBookAI</h1>
                {returnBaseSkeletonOrCards(leagues)}
            </Col>
        </Row>
    </Container>
}

export default Home;