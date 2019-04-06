import React from 'react';
import {Modal, Typography, Grid, Button, Paper, withStyles} from '@material-ui/core';
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
    outline: 'none',
  },
  message: {
    padding: '15px 0 20px 0',
  }
});

class ConfirmModal extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  renderActions() {
    const {confirmButtonText, onConfirm, onCancel} = this.props;

    return (
      <Grid container>
        <Grid item xs={6}>
          <Button type="button"
            variant="outlined"
            onClick={onCancel}>
            Cancel
          </Button>
        </Grid>
        <Grid item xs={6} align="right">
          <Button type="submit"
            variant="contained"
            color="primary"
            onClick={onConfirm}>
            {confirmButtonText}
          </Button>
        </Grid>
      </Grid>
    )
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
          <Typography variant="h4">{title}</Typography>
          <Typography variant="subtitle1" className={classes.message}>{message}</Typography>
          {this.renderActions()}
        </Paper>
      </Modal>
    );
  }
}

ConfirmModal.propTypes = {
  open: PropTypes.bool.isRequired,
  title: PropTypes.string,
  message: PropTypes.string,
  confirmButtonText: PropTypes.string,
  onConfirm: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

ConfirmModal.defaultProps = {
  title: 'Confirm',
  message: 'Are you sure?',
  confirmButtonText: 'Confirm',
}

export default withStyles(styles)(ConfirmModal);
