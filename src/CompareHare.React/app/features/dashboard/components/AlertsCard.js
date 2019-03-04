import React from 'react';
import {
  withStyles,
  Card,
  CardContent,
  CardActions,
  Button,
  Typography,
} from '@material-ui/core';
import PropTypes from 'prop-types';

const styles = {
  card: {
    minWidth: 400,
  },
};

class AlertsCard extends React.Component {
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
          <Button size="small">Add one now!</Button>
        </CardActions>
      </Card>
    );
  }
}

AlertsCard.PropTypes = {
  classes: PropTypes.object.required,
};

export default withStyles(styles)(AlertsCard);
