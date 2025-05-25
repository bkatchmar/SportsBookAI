import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import League from './pages/League';
import LeagueTeamData from './pages/LeagueTeamData'

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/:leagueName" element={<League />} />
        <Route path="/:leagueName/:teamName" element={<LeagueTeamData />} />
      </Routes>
    </Router>
  );
}

export default App