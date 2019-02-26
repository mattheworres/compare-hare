import autobind from 'class-autobind';
import React, {Component} from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import {Route, Redirect} from 'react-router-dom';
import {isAuthenticatedSelector} from '../../authentication/selectors/currentUser';

class AuthenticatedRoute extends Component {
  constructor(props, context) {
    super(props, context);

    autobind(this);
  }

  routeRenderer(routeProps) {
    const {component: Component, render, isAuthenticated} = this.props; // eslint-disable-line react/prop-types

    if (!isAuthenticated) {
      return (
        <Redirect
          to={{pathname: '/load-identity', state: {from: routeProps.location}}}
        />
      );
    }

    if (Component) return <Component {...routeProps} />;
    if (render) return render(routeProps);

    return <Redirect to={{pathname: '/not-authorized'}} />;
  }

  render() {
    const {path, exact, strict, location} = this.props;

    return (
      <Route
        path={path}
        exact={exact}
        strict={strict}
        location={location}
        render={this.routeRenderer}
      />
    );
  }
}

AuthenticatedRoute.propTypes = {
  path: PropTypes.string,
  exact: PropTypes.bool,
  strict: PropTypes.bool,
  component: PropTypes.func,
  render: PropTypes.func,
  location: PropTypes.object,
};

function mapStateToProps(state) {
  return {
    isAuthenticated: isAuthenticatedSelector(state),
  };
}

const mapDispatchToProps = {};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(AuthenticatedRoute);
