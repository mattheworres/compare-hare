import {connect} from 'react-redux';
import {logOut} from '../actions/currentUser';
import React, {PureComponent} from 'react';

const signinPage = '/signin';

class LogoutPage extends PureComponent {
  componentDidMount() {
    const {logOut, history} = this.props;

    logOut().then(() => history.push(signinPage));
  }

  render() {
    return <div>Uh, logout here</div>;
  }
}

export default connect(
  null,
  {
    logOut,
  },
)(LogoutPage);
