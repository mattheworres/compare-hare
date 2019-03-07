import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import {connect} from 'react-redux';
import Typography from '@material-ui/core/Typography';
import {withStyles, IconButton, Menu, MenuItem} from '@material-ui/core';
import {AccountCircle} from '@material-ui/icons';
import PropTypes from 'prop-types';
import {
  isAuthenticatedSelector,
  currentUserSelector,
} from '../../authentication/selectors/currentUser';

const styles = {
  title: {
    flex: 1,
  },
  bar: {
    margin: 0,
    marginBottom: 20,
  },
};

class NavBar extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      anchorElement: null,
    };

    this.handleClose = this.handleClose.bind(this);
    this.handleMenu = this.handleMenu.bind(this);
  }

  handleClose() {
    this.setState({anchorElement: null});
  }

  handleMenu(event) {
    this.setState({anchorElement: event.currentTarget});
  }

  render() {
    const {classes, isAuthenticated, currentUser} = this.props;
    const {anchorElement} = this.state;
    const open = Boolean(anchorElement);

    return (
      <div>
        <AppBar position="static" className={classes.bar}>
          <Toolbar>
            <Typography
              className={classes.title}
              variant="title"
              color="inherit"
            >
              CompareHare (Alpha)
            </Typography>
            {isAuthenticated && (
              <Typography color="inherit">
                {currentUser.firstName} {currentUser.lastName.charAt(0)}.&nbsp;
              </Typography>
            )}
            {isAuthenticated && (
              <div>
                <IconButton
                  aria-owns={open ? 'menu-appbar' : undefined}
                  aria-haspopup="true"
                  onClick={this.handleMenu}
                  color="inherit"
                >
                  <AccountCircle />
                </IconButton>
                <Menu
                  id="menu-appbar"
                  anchorEl={anchorElement}
                  anchorOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                  }}
                  transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                  }}
                  open={open}
                  onClose={this.handleClose}
                >
                  <MenuItem onClick={this.handleClose}>My account</MenuItem>
                  <MenuItem onClick={this.handleClose}>Log out</MenuItem>
                </Menu>
              </div>
            )}
          </Toolbar>
        </AppBar>
      </div>
    );
  }
}

NavBar.propTypes = {
  classes: PropTypes.object.required,
};

function mapStateToProps(state) {
  return {
    isAuthenticated: isAuthenticatedSelector(state),
    currentUser: currentUserSelector(state),
  };
}

const mapDispatchToProps = {};

export default withStyles(styles)(
  connect(
    mapStateToProps,
    mapDispatchToProps,
  )(NavBar),
);
