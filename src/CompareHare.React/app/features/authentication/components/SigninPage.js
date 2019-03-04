import React from 'react';
import {connect} from 'react-redux';
import PropTypes from 'prop-types';
import withStyles from '@material-ui/core/styles/withStyles';
//import {boundMethod} from 'autobind-decorator';
import CssBaseline from '@material-ui/core/CssBaseline';
import {SigninForm} from './index';
import {
  submittingSelector,
  validationErrorsSelector,
} from '../selectors/signin';
import {authenticate} from '../actions/currentUser';
import {signIn} from '../actions/signin';
import {handleApiError} from '../../shared/services';
import autobind from 'class-autobind';

const defaultPageAfterSignin = '/dashboard';

const styles = theme => ({
  main: {
    width: 'auto',
    display: 'block', // Fix IE 11 issue.
    marginLeft: theme.spacing.unit * 3,
    marginRight: theme.spacing.unit * 3,
    [theme.breakpoints.up(400 + theme.spacing.unit * 3 * 2)]: {
      width: 400,
      marginLeft: 'auto',
      marginRight: 'auto',
    },
  },
  paper: {
    marginTop: theme.spacing.unit * 8,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    padding: `${theme.spacing.unit * 2}px ${theme.spacing.unit * 3}px ${theme
      .spacing.unit * 3}px`,
  },
  avatar: {
    margin: theme.spacing.unit,
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: '100%', // Fix IE 11 issue.
    marginTop: theme.spacing.unit,
  },
  submit: {
    marginTop: theme.spacing.unit * 3,
  },
});

class SigninPage extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      email: '',
      password: '',
      rememberMe: false,
    };

    autobind(this);
  }

  componentDidMount() {
    this.setState({email: '', password: '', rememberMe: false});
  }

  //@boundMethod
  handleFieldChange(event) {
    const {name, value} = event.target;
    this.setState({[name]: value});
  }

  //@boundMethod
  handleSubmit(event) {
    event.preventDefault();

    const {authenticate, history, location, signIn} = this.props;
    const {email, password} = this.state;

    const model = {email, password};

    signIn(model)
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
          history.push(defaultPageAfterSignin);
        }
      })
      .catch(error =>
        handleApiError(
          error,
          history,
          'An error occurred while attempting to sign in.',
          'Error',
        ),
      );
  }

  render() {
    const {classes, validationErrors} = this.props;

    return (
      <main className={classes.main}>
        <CssBaseline />
        <SigninForm
          validationErrors={validationErrors}
          onFieldChange={this.handleFieldChange}
          onSubmit={this.handleSubmit}
        />
      </main>
    );
  }
}

SigninPage.propTypes = {
  classes: PropTypes.object.isRequired,
};

function mapStateToProps(state) {
  return {
    submitting: submittingSelector(state),
    validationErrors: validationErrorsSelector(state),
  };
}

const mapDispatchToProps = {
  signIn,
  authenticate,
};

export default withStyles(styles)(
  connect(
    mapStateToProps,
    mapDispatchToProps,
  )(SigninPage),
);
