using PX.Data;
using PX.Data.WorkflowAPI;
using System.Collections;

namespace PX.Objects.GL
{
    public class JournalEntry_Extensions : PXGraphExtension<JournalEntry_Workflow, JournalEntry>
    {
        #region Override Methods
     //   public override void Configure(PXScreenConfiguration config)
     //   {
     //       Configure(config.GetScreenConfigurationContext<JournalEntry, Batch>());
     //   }

     //   protected virtual void Configure(WorkflowContext<JournalEntry, Batch> context)
     //   {
     //       context.UpdateScreenConfigurationFor(screen =>
     //           screen.UpdateDefaultFlow(flow =>
     //               flow.WithTransitions(transitions =>
					//{
					//	transitions.UpdateGroupFrom<BatchStatus.hold>(ts =>
					//	{
					//		ts.Remove(t => t
					//			           .To<BatchStatus.balanced>()
					//				       .IsTriggeredOn(g => g.releaseFromHold)
     //                                      .DoesNotPersist()
     //                                      .WithFieldAssignments(fas => fas.Add<Batch.hold>(f => f.SetFromValue(false))));
					//		ts.Add(t => t
					//				   .To<BatchStatus.balanced>()
					//				   .IsTriggeredOn(g => g.releaseFromHold)
     //                                  .PlaceFirst()
					//				   .WithFieldAssignments(fas => fas.Add<Batch.hold>(f => f.SetFromValue(false))));
					//	});
					//})
     //            )
     //       );
     //   }
        #endregion

        #region Delegate Methods
        public delegate IEnumerable ReleaseFromHoldDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable ReleaseFromHold(PXAdapter adapter, ReleaseFromHoldDelegate baseMethod)
        {
            Base.Save.Press();

            return baseMethod(adapter);
        }
        #endregion
    }
}
