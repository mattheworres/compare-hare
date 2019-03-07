import React from 'react';
import {Page} from '../../layout/components';
import {Link} from 'react-router-dom';

class LandingPage extends React.Component {
  render() {
    return (
      <Page>
        <div>
          TODO: a vertical stepper on MD-XL screens and mobile stepper on XL-SM
          screens basically selling why CompareHare is awesome (and mainly what
          it&apos;s used for)
        </div>
        <div>Also, TODO: Make NavBar respect isAuthenticated</div>
        <br />
        <div>
          You wanna go here: <Link to="/dashboard">Dashboard</Link>
        </div>
      </Page>
    );
  }
}

export default LandingPage;
