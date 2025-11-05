<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ScientificToNumber">
		<xsl:param name="ScientificN"/>
		<xsl:variable name="vExponent" select="substring-after($ScientificN,'E')"/>
		<xsl:variable name="vMantissa" select="substring-before($ScientificN,'E')"/>
		<xsl:variable name="vFactor"
				 select="substring('100000000000000000000000000000000000000000000',
                              1, substring($vExponent,2) + 1)"/>
		<xsl:choose>
			<xsl:when test="starts-with($vExponent,'-')">
				<xsl:value-of select="$vMantissa div $vFactor"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$vMantissa * $vFactor"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<!--<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>-->

				<RecordTypeCode>
					<xsl:value-of select="'RecordTypeCode'"/>
				</RecordTypeCode>

				<AssetType>
					<xsl:value-of select="'AssetType'"/>
				</AssetType>



				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>
			
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:choose>
					<xsl:when test="TaxlotState!='Amended'">
						
					</xsl:when>
					<xsl:otherwise>
					<xsl:if test ="number(OldExecutedQuantity)">

					<ThirdPartyFlatFileDetail>

								<!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<FileHeader>
									<xsl:value-of select="'true'"/>
								</FileHeader>

								<FileFooter>
									<xsl:value-of select="'true'"/>
								</FileFooter>
								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>


								<RecordTypeCode>
									<xsl:value-of select="'TRN'"/>
								</RecordTypeCode>

								<AssetType>
									<xsl:choose>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'STOCK'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="Asset='FixedIncome'">
													<xsl:value-of select="'BOND'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</AssetType>





								<!-- system use only-->
								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>

					
						<ThirdPartyFlatFileDetail>

							<!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<!--<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter>-->
							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>


							<RecordTypeCode>
								<xsl:value-of select="'TRN'"/>
							</RecordTypeCode>

							<AssetType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="'BOND'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</AssetType>




							<!-- system use only-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					<!--</xsl:otherwise>-->


					</xsl:otherwise>
				</xsl:choose>

				
			</xsl:for-each>
			
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
