<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>




	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) ">
					
					<PositionMaster>					
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BTIG'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

					

						<xsl:variable name="AssetType">
							<xsl:choose>

								<xsl:when test="contains(COL7,'CALL') or contains(COL7,'PUT')">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
							

								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="Symbol" select="normalize-space(COL5)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="string-length(COL5) &gt; 20">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL5!='*'">
									<xsl:value-of select="COL5"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="string-length(COL5) &gt; 20">
									<xsl:value-of select="concat(COL5, 'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select="COL1"/>
						</PositionStartDate>


						<xsl:variable name="varSide" select="COL2"/>

						<SideTagValue>
						
							
							<xsl:choose>

								<xsl:when test="$AssetType='Option'">
									<xsl:choose>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BUY'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BTC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SEL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SSL'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>


									</xsl:choose>
								</xsl:when>



								<xsl:otherwise>


									<xsl:choose>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BTC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SEL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SSL'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='STO'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='STC'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BTO'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>


									</xsl:choose>


								</xsl:otherwise>

							</xsl:choose>

						</SideTagValue>


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


						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="translate(COL9,',','')"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Commission">
							<xsl:value-of select="translate(translate(COL10,',',''),'(','')"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ='$Commission*-1'/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="Fees">
							<xsl:value-of select="translate(COL11,',','')"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($Fees) &lt; 0'>
									<xsl:value-of select ='$Fees*-1'/>
								</xsl:when>
								<xsl:when test ='number($Fees) &gt; 0'>
									<xsl:value-of select ='$Fees'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="PB_COUNTER_PARTY" select="COL17"/>

						<xsl:variable name="PRANA_COUNTER_PARTY">
							<xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTER_PARTY]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>

								<xsl:when test ="number($PRANA_COUNTER_PARTY) ">
									<xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CounterPartyID>



						<!--<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>


</xsl:stylesheet>
