import React from 'react';
import {
  withStyles,
  Card,
  CardContent,
  CardActions,
  Button,
  Typography,
} from '@material-ui/core';
import {connect} from 'react-redux';
import PropTypes from 'prop-types';
import {openAddAlert} from '../../alerts/actions/addAlert';
import autobind from 'class-autobind';

const styles = {
  card: {
    minWidth: 400,
  },
};

class AlertsCard extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  handleAddClick() {
    this.props.openAddAlert();
  }

  render() {
    const {classes} = this.props;
    return (
      <Card className={classes.card}>
        <CardContent>
          <Typography variant="h5" component="h2">
            You have no alerts!
          </Typography>
          <Typography component="p">Get started by clicking below!</Typography>
        </CardContent>
        <CardActions>
          <Button size="small" onClick={this.handleAddClick}>Add one now!</Button>
        </CardActions>
      </Card>
    );
  }
}

AlertsCard.propTypes = {
  classes: PropTypes.object,
};

const mapDispatchToProps = {
  openAddAlert,
}

export default connect(null, mapDispatchToProps)(withStyles(styles)(AlertsCard));
