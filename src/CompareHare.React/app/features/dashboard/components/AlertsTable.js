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
  TableFooter,
} from '@material-ui/core';
import {Link} from 'react-router-dom';
import moment from 'moment';
import {connect} from 'react-redux';
import PropTypes from 'prop-types';
import {loadAlerts, deleteAlert} from '../actions/alertsTable';
import {openAddAlert} from '../../alerts/actions/addAlert';
import {openEditAlert} from '../../alerts/actions/editAlert';
import autobind from 'class-autobind';
import {
  loadingSelector,
  hasErrorSelector,
  alertsSelector,
  deletingSelector
} from '../selectors/alertsTable';
import {loadingSelector as loadingEditSelector} from '../../alerts/selectors/editAlert';
import {Delete, Edit, Add} from '@material-ui/icons';
import {retrieveAttributeValue} from '../../shared/services/displayHelpers';
import {ConfirmModal, LoadingModal} from '../../shared/components';
import toastr from 'toastr';

const styles = theme => ({
  card: {
    minWidth: 400,
  },
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  paddedTypography: {
    padding: '15px',
  }
});

class AlertsTable extends React.Component {
  constructor(props) {
    super(props);

    autobind(this);

    this.state = {
      alertId: null,
      showDeleteConfirm: false,
    };
  }

  componentDidMount() {
    this.props.loadAlerts();
  }

  handleAddClick() {
    this.props.openAddAlert();
  }

  handleEditClick() {
    const {openEditAlert} = this.props;
    const alertId = retrieveAttributeValue(event, 'data-alert-id');

    openEditAlert(alertId);
  }

  handleDeleteClick(event) {
    const alertId = retrieveAttributeValue(event, 'data-alert-id');

    this.setState({
      showDeleteConfirm: true,
      alertId: alertId,
    });
  }

  confirmDeleteAlert() {
    const {alertId} = this.state;

    this.setState({showDeleteConfirm: false});

    this.props.deleteAlert(alertId).then(() => {
      this.setState({alertId: null});
      toastr.success('Alert deleted!');
      this.props.loadAlerts();
    }).catch(() => {
      toastr.error('Crap!', 'Couldnt delete your alert, sorry');
    });
  }

  renderEmptyContent() {
    const {classes} = this.props;
    return (
      <Card className={classes.card}>
        <CardContent>
          <Typography variant="h5" component="h2" className={this.props.paddedTypography}>
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
        <Typography variant="h5" className={this.props.paddedTypography}>Loading...</Typography>
      </Paper>
    );
  }

  renderError() {
    return (
      <Paper>
        <Typography variant="h5" className={this.props.paddedTypography}>Whoops, we can&apos;t load your alerts...</Typography>
      </Paper>
    )
  }

  renderAlert(alert) {
    return (
      <TableRow key={alert.id}>
        <TableCell><Link to={`/alerts/${alert.id}/display`}>{alert.name}</Link></TableCell>
        <TableCell>{alert.utilityState}</TableCell>
        <TableCell>{alert.utilityType}</TableCell>
        <TableCell>{moment(alert.lastEdited).fromNow()}</TableCell>
        <TableCell align="center">{alert.matchesCount}</TableCell>
        <TableCell align="right">
          <Fab size="medium" color="primary" onClick={this.handleEditClick} data-alert-id={alert.id}><Edit /></Fab>&nbsp;
          <Fab size="medium" color="secondary" onClick={this.handleDeleteClick} data-alert-id={alert.id}><Delete /></Fab>
        </TableCell>
      </TableRow>
    )
  }

  render() {
    const {classes, alerts, loading, loadingEdit, deleting, hasError} = this.props;
    const {showDeleteConfirm} = this.state;

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
          <TableFooter>
            <TableRow>
              <TableCell colSpan={6}>
                <Button size="small" color="primary" onClick={this.handleAddClick}><Add />&nbsp;Add a new alert</Button>
              </TableCell>
            </TableRow>
          </TableFooter>
        </Table>
        <ConfirmModal
          open={showDeleteConfirm}
          title="Delete Alert"
          confirmButtonText="Delete"
          onCancel={() => this.setState({showDeleteConfirm: false, alertId: null})}
          onConfirm={this.confirmDeleteAlert} />
        <LoadingModal open={loading || deleting} />
        <LoadingModal open={loadingEdit} message="Loading alert..." />
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
    loadingEdit: loadingEditSelector(state),
    deleting: deletingSelector(state),
    hasError: hasErrorSelector(state),
    alerts: alertsSelector(state),
  };
}

const mapDispatchToProps = {
  loadAlerts,
  openAddAlert,
  openEditAlert,
  deleteAlert,
}

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(AlertsTable));
