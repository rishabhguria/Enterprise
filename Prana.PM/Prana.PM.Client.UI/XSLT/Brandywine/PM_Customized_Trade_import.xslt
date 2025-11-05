<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(translate(COL55,',',''))">
				<PositionMaster>

					<!--<AccountName>
            <xsl:value-of select="''"/>
          </AccountName>-->

					<xsl:variable name="Symbol">
						<xsl:value-of select="COL43"/>
					</xsl:variable>
					<xsl:variable name="varSedol">
						<xsl:value-of select="COL40"/>
					</xsl:variable>
					<Symbol>
						<xsl:choose>

							
						<xsl:when test="$varSedol!='*'">
							<xsl:value-of select="''"/>
						</xsl:when>


							<xsl:when test="$Symbol!='*'">
								<xsl:value-of select="$Symbol"/>
							</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<SEDOL>
						<xsl:choose>
							<xsl:when test="$varSedol!='*'">
								<xsl:value-of select="$varSedol"/>
							</xsl:when>

							<xsl:when test="$Symbol!='*'">
								<xsl:value-of select="''"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>		
					</SEDOL>

					<PBSymbol>
						<xsl:value-of select="COL8"/>
					</PBSymbol>


					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'State Street'"/>
					</xsl:variable>

				
					
					<xsl:variable name="PB_FUND_NAME" select="''"/>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name='State Street']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>
							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="$PB_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountName>
					
					
					<xsl:variable name="CostBasis">
						<xsl:value-of select="COL56"/>
					</xsl:variable>
					<CostBasis>
						<xsl:choose>
							<xsl:when test="$CostBasis &gt; 0">
								<xsl:value-of select="$CostBasis"/>
							</xsl:when>
							<xsl:when test="$CostBasis &lt; 0">
								<xsl:value-of select="$CostBasis * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CostBasis>
					
					
					<xsl:variable name="Commission">
						<xsl:value-of select="COL59"/>
					</xsl:variable>
					<Commission>
						<xsl:choose>
							<xsl:when test="$Commission &gt; 0">
								<xsl:value-of select="$Commission"/>
							</xsl:when>
							<xsl:when test="$Commission &lt; 0">
								<xsl:value-of select="$Commission * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>
					
					<xsl:variable name="Position">
						<xsl:value-of select="translate(COL55,',','')"/>
					</xsl:variable>
					
					<NetPosition>
						<xsl:choose>
							<xsl:when test="$Position &gt; 0">
								<xsl:value-of select="$Position"/>
							</xsl:when>
							<xsl:when test="$Position &lt; 0">
								<xsl:value-of select="$Position * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>
					
					<xsl:variable name="Side">
						<xsl:value-of select="COL54"/>
					</xsl:variable>
					<SideTagValue>
						<xsl:choose>
							
								
									<xsl:when test="$Side='PURCHASE'">
										<xsl:value-of select="'1'"/>
									</xsl:when>
									<xsl:when test="$Side='SALE'">
										<xsl:value-of select="'2'"/>
									</xsl:when>
									<xsl:when test="$Side='SellShort'">
										<xsl:value-of select="'5'"/>
									</xsl:when>
									<xsl:when test="$Side='CoverShort'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
							
							
						</xsl:choose>


					</SideTagValue>


					<xsl:variable name="Fees">
						<xsl:value-of select="COL62"/>
					</xsl:variable>
					<Fees>
						<xsl:choose>
							<xsl:when test="$Fees &gt; 0">
								<xsl:value-of select="$Fees"/>
							</xsl:when>
							<xsl:when test="$Fees &lt; 0">
								<xsl:value-of select="$Fees * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Fees>

				
					
					<PositionStartDate>
						<xsl:value-of select="COL44"/>
					</PositionStartDate>
				
					<FXRate>
						<xsl:value-of select="COL58"/>
					</FXRate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>
