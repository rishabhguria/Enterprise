<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[Account ='Alpha Omega JPM: 420-29971']">

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
						<xsl:value-of select="'AO'"/>
					</Strategy>

					<AOSector>
						<xsl:value-of select="AOSector"/>
					</AOSector>

					<AOSubSector>
						<xsl:value-of select="AOSubSector"/>
					</AOSubSector>

					<AOValue>
						<xsl:value-of select="AOValue"/>
					</AOValue>

					<AOWWHAssessment>
						<xsl:value-of select="AOWWHAssessment"/>
					</AOWWHAssessment>

					<AOSCPSizeBand>
						<xsl:value-of select="AOSCPSizeBand"/>
					</AOSCPSizeBand>


					<AOAspirationalSCPSizeBand>
						<xsl:value-of select="AOAspirationalSCPSizeBand"/>
					</AOAspirationalSCPSizeBand>

					<AOMinSize>
						<xsl:value-of select="AOMinSize"/>
					</AOMinSize>

					<AOMidPointSize>
						<xsl:value-of select="AOMidPointSize"/>
					</AOMidPointSize>

					<AOMaxSize>
						<xsl:value-of select="AOMaxSize"/>
					</AOMaxSize>

					<AOLatestTargetSize>
						<xsl:value-of select="AOLatestTargetSize"/>
					</AOLatestTargetSize>

					<AOModelTargetSize>
						<xsl:value-of select="AOModelTargetSize"/>
					</AOModelTargetSize>

					<AORawIVPrice>
						<xsl:value-of select="AORawIVPrice"/>
					</AORawIVPrice>

					<AOConf-AdjIVPrice>
						<xsl:value-of select="AOConfAdjIVPrice"/>
					</AOConf-AdjIVPrice>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>