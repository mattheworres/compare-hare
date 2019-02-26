import autobind from 'class-autobind';
import {connect} from 'react-redux';
import {LoginForm} from './index';
import {
  loginModelSelector,
  submittingSelector,
  validationErrorsSelector,
} from '../selectors/login';
import {authenticate} from '../actions/currentUser';
import {login, resetLoginForm, setLoginFormField} from '../actions/login';
import {Grid, Header} from 'semantic-ui-react';
import React, {Component} from 'react';
import {handleApiError} from '../../shared/services';

const defaultPageAfterLogin = '/dashboard';

class LoginPage extends Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  componentDidMount() {
    this.props.resetLoginForm();
  }

  handleFieldChange(event) {
    const {name, value} = event.target;
    this.props.handleFieldChange(name, value);
  }

  handleSubmit(event) {
    event.preventDefault();

    const {authenticate, history, location, login, model} = this.props;

    login(model)
      .then(response => {
        authenticate(response.value);

        let redirectTo;

        if (location.state) {
          const {from} = location.state;
          if (from && from.pathname) redirectTo = from.pathname;
        }

        if (redirectTo) {
          history.push(redirectTo);
        } else {
          history.push(defaultPageAfterLogin);
        }
      })
      .catch(error =>
        handleApiError(
          error,
          history,
          'An error occurred while attempting to Login.',
          'Error',
        ),
      );
  }

  render() {
    const {model, submitting, validationErrors} = this.props;

    return (
      <Grid
        textAlign="center"
        className="loginContainer"
        verticalAlign="middle"
      >
        <Grid.Column style={{maxWidth: 450}}>
          <Header as="h2" textAlign="center">
            Login
          </Header>
          <LoginForm
            model={model}
            disabled={submitting}
            validationErrors={validationErrors}
            onFieldChange={this.handleFieldChange}
            onSubmit={this.handleSubmit}
          />
        </Grid.Column>
      </Grid>
    );
  }
}

function mapStateToProps(state) {
  return {
    model: loginModelSelector(state),
    submitting: submittingSelector(state),
    validationErrors: validationErrorsSelector(state),
  };
}

const mapDispatchToProps = {
  resetLoginForm,
  handleFieldChange: setLoginFormField,
  login,
  authenticate,
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(LoginPage);
