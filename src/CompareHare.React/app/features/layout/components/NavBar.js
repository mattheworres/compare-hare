import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import {withStyles} from '@material-ui/core';
import PropTypes from 'prop-types';

const styles = {
  bar: {
    marginBottom: 20,
  },
};

class NavBar extends React.Component {
  render() {
    const {classes} = this.props;
    return (
      <div>
        <AppBar position="static" className={classes.bar}>
          <Toolbar>
            <Typography variant="title" color="inherit">
              CompareHare (Alpha)
            </Typography>
          </Toolbar>
        </AppBar>
      </div>
    );
  }
}

NavBar.propTypes = {
  classes: PropTypes.object.required,
};

export default withStyles(styles)(NavBar);
