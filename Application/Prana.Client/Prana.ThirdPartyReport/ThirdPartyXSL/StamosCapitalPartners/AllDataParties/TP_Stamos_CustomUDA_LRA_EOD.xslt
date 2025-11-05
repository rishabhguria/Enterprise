<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[Account ='LRA: 420-64280']">

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
						<xsl:value-of select="'LRA'"/>
					</Strategy>

					<LRASector>
						<xsl:value-of select="LRASector"/>
					</LRASector>

					<LRASubSector>
						<xsl:value-of select="LRASubSector"/>
					</LRASubSector>

					<LRAValue>
						<xsl:value-of select="LRAValue"/>
					</LRAValue>

					<LRAWWHAssessment>
						<xsl:value-of select="LRAWWHAssessment"/>
					</LRAWWHAssessment>

					<LRASCPSizeBand>
						<xsl:value-of select="LRASCPSizeBand"/>
					</LRASCPSizeBand>


					<LRAAspirationalSCPSizeBand>
						<xsl:value-of select="LRAAspirationalSCPSizeBand"/>
					</LRAAspirationalSCPSizeBand>

					<LRAMinSize>
						<xsl:value-of select="LRAMinSize"/>
					</LRAMinSize>

					<LRAMidPointSize>
						<xsl:value-of select="LRAMidPointSize"/>
					</LRAMidPointSize>

					<LRAMaxSize>
						<xsl:value-of select="LRAMaxSize"/>
					</LRAMaxSize>

					<LRALatestTargetSize>
						<xsl:value-of select="LRALatestTargetSize"/>
					</LRALatestTargetSize>

					<LRAModelTargetSize>
						<xsl:value-of select="LRAModelTargetSize"/>
					</LRAModelTargetSize>

					<LRARawIVPrice>
						<xsl:value-of select="LRARawIVPrice"/>
					</LRARawIVPrice>

					<LRAConf-AdjIVPrice>
						<xsl:value-of select="LRAConfAdjIVPrice"/>
					</LRAConf-AdjIVPrice>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>