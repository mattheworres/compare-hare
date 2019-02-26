import {connect} from 'react-redux';
import {logOut} from '../actions/currentUser';
import {Segment, Dimmer, Modal} from 'semantic-ui-react';
import React, {PureComponent} from 'react';

const loginPage = '/login';

class LogoutPage extends PureComponent {
  componentDidMount() {
    const {logOut, history} = this.props;

    logOut().then(() => history.push(loginPage));
  }

  render() {
    return (
      <Segment>
        <Dimmer active inverted>
          <Modal open className="logoutContainer">
            <Modal.Content>
              <p className="logoutMessage">You have been logged out.</p>
            </Modal.Content>
          </Modal>
        </Dimmer>
      </Segment>
    );
  }
}

export default connect(
  null,
  {
    logOut,
  },
)(LogoutPage);
