<?xml version="1.0" encoding="utf-8" ?>
<!--Description Sansato FX and FXForward Import-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="tempMonth">
		<xsl:param name="paramMonthCode"/>
		<xsl:choose>
			<xsl:when test="$paramMonthCode='01'">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='02'">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='03'">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='04'">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='05'">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='06'">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='07'">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='08'">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='09'">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='10'">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='11'">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$paramMonthCode='12'">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="COL1 != 'tradedate'">
					<PositionMaster>

						<xsl:variable name="varFxSymbol">
							<xsl:if test="COL6='SPT' or COL6='FRD'">
								<xsl:value-of select="concat(COL2,'-USD')"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="varFxForwardMonthCode">
							<xsl:if test="COL6='NDF'">
								<xsl:value-of select="substring-before(COL15,'/')"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="varFxForwardMonthCodeFormated">
							<xsl:choose>
								<xsl:when test ="$varFxForwardMonthCode=1">
									<xsl:value-of select="'01'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=2">
									<xsl:value-of select="'02'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=3">
									<xsl:value-of select="'03'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=4">
									<xsl:value-of select="'04'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=5">
									<xsl:value-of select="'05'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=6">
									<xsl:value-of select="'06'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=7">
									<xsl:value-of select="'07'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=8">
									<xsl:value-of select="'08'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardMonthCode=9">
									<xsl:value-of select="'09'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFxForwardMonthCode"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFxForwardMonth">
							<xsl:if test="COL6='NDF'">
								<xsl:call-template name="tempMonth">
									<xsl:with-param name="paramMonthCode" select="$varFxForwardMonthCodeFormated"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="varFxForwardDate1">
							<xsl:if test="COL6='NDF'">
								<xsl:value-of select="substring-before(substring-after(COL15,'/'),'/')"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="varFxForwardDate">
							<xsl:choose>
								<xsl:when test ="$varFxForwardDate1=1">
									<xsl:value-of select="'01'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=2">
									<xsl:value-of select="'02'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=3">
									<xsl:value-of select="'03'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=4">
									<xsl:value-of select="'04'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=5">
									<xsl:value-of select="'05'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=6">
									<xsl:value-of select="'06'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=7">
									<xsl:value-of select="'07'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=8">
									<xsl:value-of select="'08'"/>
								</xsl:when>
								<xsl:when test ="$varFxForwardDate1=9">
									<xsl:value-of select="'09'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFxForwardDate1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFxForwardYear">
							<xsl:if test="COL6='NDF'">
								<xsl:value-of select="substring-after(substring-after(COL15,'/'),'/')"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="varFxForwardSymbol">
							<xsl:if test="COL6='NDF'">
								<xsl:value-of select="concat(COL2,'-USD',' ',$varFxForwardDate,$varFxForwardMonth,$varFxForwardYear)"/>
							</xsl:if>
						</xsl:variable>


						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME !=''">
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL6='NDF'">
								<Symbol>
									<xsl:value-of select="$varFxForwardSymbol"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="COL6='SPT' or COL6='FRD'">
								<Symbol>
									<xsl:value-of select="$varFxSymbol"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<!--Cost Basis Indecate Price-->
						<xsl:choose>
							<xsl:when  test="boolean(number(COL11))">
								<CostBasis>
									<xsl:value-of select="COL11"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >

						<!-- In the format of mm_dd_yyyy-->
						<PositionStartDate>
							<xsl:value-of select="concat(substring-before(COL1,'/'),'/',substring-before(substring-after(COL1,'/'),'/'),'/',substring(substring-after(substring-after(COL1,'/'),'/'),3,2))"/>
						</PositionStartDate>


						<!--BEGIN FOR NET POSITION ie QUANTITY -->
						<xsl:choose>
							<xsl:when  test="number(COL10) &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL10 *(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when  test="number(COL10) &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL10 "/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<!--BEGIN FOR BUY AND SELL DESCRIPTION -->
						<xsl:choose>
							<xsl:when test="COL5 ='BL'">
								<SideTagValue>
									<xsl:value-of select="1"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL5='BC'">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL5='SL'">
								<SideTagValue>
									<xsl:value-of select="2"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL5='SS'">
								<SideTagValue>
									<xsl:value-of select="5"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="0"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="EB_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<!-- Sandeep Singh: Only one Counter Party for FX so I have hard code Counter Party ID here only-->
						<!--<xsl:variable name="PRANA_EB_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>-->

						<xsl:choose>
							<xsl:when test="COL4 ='DB'">
								<CounterPartyID>
									<xsl:value-of select="1"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select="0"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>


						<PBSymbol>
							<xsl:value-of select="COL2"/>
						</PBSymbol>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


