<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[Account ='DVP: 420-64281']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<EfffectiveDate>
						<xsl:value-of select="EffectiveDate"/>
					</EfffectiveDate>

					<PK>
						<xsl:value-of select="BloombergSymbol"/>
					</PK>

					<Strategy>
						<xsl:value-of select="'DVP'"/>
					</Strategy>

					<DVPSector>
						<xsl:value-of select="DVPSector"/>
					</DVPSector>

					<DVPSubSector>
						<xsl:value-of select="DVPSubSector"/>
					</DVPSubSector>

					<DVPValue>
						<xsl:value-of select="DVPValue"/>
					</DVPValue>

					<DVPWWHAssessment>
						<xsl:value-of select="DVPWWHAssessment"/>
					</DVPWWHAssessment>

					<DVPSCPSizeBand>
						<xsl:value-of select="DVPSCPSizeBand"/>
					</DVPSCPSizeBand>


					<DVPAspirationalSCPSizeBand>
						<xsl:value-of select="DVPAspirationalSCPSizeBand"/>
					</DVPAspirationalSCPSizeBand>

					<DVPMinSize>
						<xsl:value-of select="DVPMinSize"/>
					</DVPMinSize>

					<DVPMidPointSize>
						<xsl:value-of select="DVPMidPointSize"/>
					</DVPMidPointSize>

					<DVPMaxSize>
						<xsl:value-of select="DVPMaxSize"/>
					</DVPMaxSize>

					<DVPLatestTargetSize>
						<xsl:value-of select="DVPLatestTargetSize"/>
					</DVPLatestTargetSize>

					<DVPModelTargetSize>
						<xsl:value-of select="DVPModelTargetSize"/>
					</DVPModelTargetSize>

					<DVPRawIVPrice>
						<xsl:value-of select="DVPRawIVPrice"/>
					</DVPRawIVPrice>

					<DVPConf-AdjIVPrice>
						<xsl:value-of select="DVPConfAdjIVPrice"/>
					</DVPConf-AdjIVPrice>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>