import React from 'react';
import {Redirect} from 'react-router-dom';

export default function RedirectToDashboard() {
  return <Redirect to="/dashboard" />;
}
