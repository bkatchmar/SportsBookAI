import React, { useState, useEffect } from 'react'
import Skeleton from 'react-loading-skeleton'
import Table from 'react-bootstrap/Table'

function BaseAggregationTable(props) {
    const [aggregatorData, setAggregatorData] = useState({})
    const displayPercent = (percent) => (percent) ? `${(percent * 100).toFixed(2)}%` : '0%'

    useEffect(() => {
        setAggregatorData(props.aggregatorData)
    }, [props.aggregatorData])

    if (Object.keys(aggregatorData).length === 0) {
        return <Skeleton count={10} />
    }

    return <Table responsive striped hover className="mb-5">
        <thead>
            <tr>
                <th>Over %</th>
                <th>Under %</th>
                <th>Spread Cover %</th>
                <th>Prevent Spread Cover %</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{displayPercent(aggregatorData["allOverPercentage"])}</td>
                <td>{displayPercent(aggregatorData["allUnderPercentage"])}</td>
                <td>{displayPercent(aggregatorData["allMinusSpreadsPercentage"])}</td>
                <td>{displayPercent(aggregatorData["allPlusSpreadsPercentage"])}</td>
            </tr>
        </tbody>
    </Table>
}

export default BaseAggregationTable