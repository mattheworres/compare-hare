import autobind from 'class-autobind';
import {connect} from 'react-redux';
import {loadIdentity} from '../actions/currentUser';
import {Page} from '../../layout/components';
import React, {Component} from 'react';

class LoadIdentityPage extends Component {
  constructor(props, context) {
    super(props, context);

    autobind(this);
  }

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
            history.push('/login', {state: {from}});
          }
        } else {
          history.push('/login');
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
