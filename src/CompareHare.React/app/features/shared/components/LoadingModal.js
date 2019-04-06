import React from 'react';
import {Modal, Typography, CircularProgress, withStyles, Paper} from '@material-ui/core';
import PropTypes from 'prop-types';
import autobind from 'class-autobind';

const styles = theme => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  paper: {
    position: 'absolute',
    width: theme.spacing.unit * 50,
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[5],
    padding: theme.spacing.unit * 4,
    textAlign: 'center',
    outline: 'none',
  },
  message: {
    padding: '15px 0 20px 0',
  }
});

class LoadingModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  render() {
    const {
      open,
      title,
      message,
      classes,
    } = this.props;

    return (
      <Modal open={open} className={classes.modal}>
        <Paper className={classes.paper}>
          <Typography variant="h3">{title}</Typography>
          <Typography variant="subtitle1" className={classes.message}>{message}</Typography>
          <CircularProgress />
        </Paper>
      </Modal>
    );
  }
}

LoadingModal.propTypes = {
  open: PropTypes.bool.isRequired,
  title: PropTypes.string,
  message: PropTypes.string,
};

LoadingModal.defaultProps = {
  title: 'Loading',
  message: 'Please wait...',
}

export default withStyles(styles)(LoadingModal);
