import React from 'react';
import {connect} from 'react-redux';
import {loadIdentity} from '../actions/currentUser';
import {Page} from '../../layout/components';

class LoadIdentityPage extends React.Component {
  componentDidMount() {
    const {loadIdentity, history, location} = this.props;

    loadIdentity()
      .then(() => {
        if (location.state) {
          const {from} = location.state;
          if (from && from.pathname) history.push(location.state.from.pathname);
          else history.push('/');
        } else {
          history.push('/');
        }
      })
      .catch(() => {
        if (location.state) {
          const {from} = location.state;
          if (from && from.pathname) {
            history.push('/signin', {state: {from}});
          }
        } else {
          history.push('/signin');
        }
      });
  }

  render() {
    return <Page loading />;
  }
}

export default connect(
  null,
  {
    loadIdentity,
  },
)(LoadIdentityPage);
