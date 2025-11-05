<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
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

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
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
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL93"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantity)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'State Street'"/>
						</xsl:variable>
						<xsl:variable name = "PB_COMPANY_NAME" >
							<xsl:value-of select="COL42"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL26"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<MasterFund>
							
									<xsl:value-of select="'PACE'"/>
								
						</MasterFund>
						
						
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
					
						<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL124)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
							
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
					
						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</Quantity>
						
						<xsl:variable name="Side" select="COL131"/>
						<Side>
							<xsl:choose>								
										<xsl:when test="$Side='BUY'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="$Side='SSELL'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>								
							</xsl:choose>
						</Side>
						
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<xsl:variable name="varMonth">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varDay">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL45,'-'))='1'">
									<xsl:value-of select="concat('0',substring-before(COL45,'-'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL45,'-')"/>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>
						
						<xsl:variable name="varYear">
							<xsl:value-of select="substring-after(substring-after(COL45,'-'),'-')"/>
						</xsl:variable>
						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>

						<xsl:variable name="varSMonth">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL36,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varSDay">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL36,'-'))='1'">
									<xsl:value-of select="concat('0',substring-before(COL36,'-'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL36,'-')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSYear">
							<xsl:value-of select="substring-after(substring-after(COL36,'-'),'-')"/>
						</xsl:variable>
						<SettlementDate>
							<xsl:value-of select="''"/>
						</SettlementDate>

						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</PBSymbol>

						<xsl:variable name="TotalCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>
						<TotalCommission>
							<xsl:choose>
								<xsl:when test="$TotalCommission &gt; 0">
									<xsl:value-of select="$TotalCommission"/>
								</xsl:when>
								<xsl:when test="$TotalCommission &lt; 0">
									<xsl:value-of select="$TotalCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommission>

						<xsl:variable name="varPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL77"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPX>
							<xsl:choose>
								<xsl:when test="$varPrice &gt; 0">
									<xsl:value-of select="$varPrice"/>
								</xsl:when>
								<xsl:when test="$varPrice &lt; 0">
									<xsl:value-of select="$varPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>


						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL76"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>
						
						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>