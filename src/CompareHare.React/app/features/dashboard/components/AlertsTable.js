import React from 'react';
import {
  withStyles,
  Card,
  CardContent,
  CardActions,
  Button,
  Typography,
  Paper,
  CircularProgress,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Fab,
} from '@material-ui/core';
import moment from 'moment';
import {connect} from 'react-redux';
import PropTypes from 'prop-types';
import {loadAlerts} from '../actions/alertsTable';
import autobind from 'class-autobind';
import loadingSelector from '../selectors/alertsTable/loadingSelector';
import hasErrorSelector from '../selectors/alertsTable/hasErrorSelector';
import alertsSelector from '../selectors/alertsTable/alertsSelector';

const styles = theme => ({
  card: {
    minWidth: 400,
  },
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
});

class AlertsTable extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);
  }

  componentDidMount() {
    this.props.loadAlerts();
  }

  handleAddClick() {
    this.props.openAddAlert();
  }

  //Left off here: Make this a table, start loading user's alerts and displaying them
  //Then, we can work on the Alert Details page to list out specifics of the alert
  //(SUI-specific components for display, similar to the summary modal in the Creator)
  //as well as fixed-width cards that fill the screen in even rows to show SUI-specific
  //matches

  renderEmptyContent() {
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

  renderLoading() {
    return (
      <Paper>
        <CircularProgress />
        <Typography variant="h5">Loading...</Typography>
      </Paper>
    );
  }

  renderError() {
    return (
      <Paper>
        <Typography variant="h5">Whoops, we can&apos;t load your alerts...</Typography>
      </Paper>
    )
  }

  renderAlert(alert) {
    return (
      <TableRow key={alert.id}>
        <TableCell>{alert.name}</TableCell>
        <TableCell>{alert.utilityState}</TableCell>
        <TableCell>{alert.utilityType}</TableCell>
        <TableCell>{moment(alert.lastEdited).fromNow()}</TableCell>
        <TableCell>{alert.matchesCount}</TableCell>
        <TableCell><Fab></Fab></TableCell>
      </TableRow>
    )
  }

  render() {
    const {classes, alerts, loading, hasError} = this.props;

    if (loading) return this.renderLoading();

    if (hasError) return this.renderError();

    if (alerts.length === 0) return this.renderEmptyContent();

    return (
      <Paper className={classes.root}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Alert Name</TableCell>
              <TableCell>State</TableCell>
              <TableCell>Utility</TableCell>
              <TableCell>Last Edited</TableCell>
              <TableCell># Matches</TableCell>
              <TableCell>&nbsp;</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {alerts.map(this.renderAlert)}
          </TableBody>
        </Table>
      </Paper>
    );
  }
}

AlertsTable.propTypes = {
  classes: PropTypes.object,
};

function mapStateToProps(state) {
  return {
    loading: loadingSelector(state),
    hasError: hasErrorSelector(state),
    alerts: alertsSelector(state),
  };
}

const mapDispatchToProps = {
  loadAlerts,
}

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(AlertsTable));
