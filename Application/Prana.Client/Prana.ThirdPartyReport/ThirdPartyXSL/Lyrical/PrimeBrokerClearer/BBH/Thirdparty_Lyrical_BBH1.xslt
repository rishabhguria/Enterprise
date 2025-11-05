<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public int RoundOff(double Qty)
		{

		return (int)Math.Round(Qty,0);
		}
	</msxsl:script>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before($Date,'/'),'/',substring-before(substring-after($Date,'/'),'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
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
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>

        <ISIN>
					<xsl:value-of select="concat('&#x0a;','ISIN')"/>
				</ISIN>

				<TYPE>
					<xsl:value-of select="'TYPE'"/>
				</TYPE>

				<UNITS>
					<xsl:value-of select="'UNITS'"/>
				</UNITS>

				<PRICE>
					<xsl:value-of select="'PRICE'"/>
				</PRICE>

				<GROSS>
				  <xsl:value-of select="'GROSS'"/>
				</GROSS>

				<COMMS>
				  <xsl:value-of select="'COMMS'"/>
				</COMMS>

				<CUSTFEE>
				  <xsl:value-of select="'CUST FEE'"/>
				</CUSTFEE>
		        
				<SECFEES>
				  <xsl:value-of select="'S.E.C. FEE'"/>
				</SECFEES>

				<NET>
				  <xsl:value-of select="'NET'"/>
				</NET>
						
				<TRADEDT>
							<xsl:value-of select="'TRADE DT'"/>
				</TRADEDT>

				<SETTLEDT>
							<xsl:value-of select="'SETTLE DT'"/>
				</SETTLEDT>

				<LOCATION>
							<xsl:value-of select="'LOCATION'"/>
			    </LOCATION>
				
				<BROKER>
					<xsl:value-of select="'BROKER'"/>
				</BROKER>
				
				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>
				
				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITY DESCRIPTION'"/>
				</SECURITYDESCRIPTION>
        
        <AccountNumber>
            <xsl:value-of select="'Account Number'"/>
        </AccountNumber>
          
        <AccountName>
            <xsl:value-of select="'Account Name'"/>
        </AccountName>
       

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
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
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<ISIN>
						<xsl:choose>
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="concat(concat('=&quot;',ISIN),'&quot;')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ISIN>

					<TYPE>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'Sell Short'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</TYPE>

					<xsl:variable name ="Qty">
						<xsl:value-of select="my:RoundOff(AllocatedQty)"/>
					</xsl:variable>
					<UNITS>
						<xsl:choose>
							<xsl:when test="number($Qty)">
								<xsl:value-of select="$Qty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</UNITS>
					
					<xsl:variable name ="varAvgPrice">
						<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
					</xsl:variable>
					<PRICE>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="$varAvgPrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PRICE>


          <xsl:variable name="varGROSS">
            <xsl:value-of select="AllocatedQty * AveragePrice"/>
          </xsl:variable>

          <xsl:variable name="GROSS">
            <xsl:value-of select="my:RoundOff($varGROSS)"/>
          </xsl:variable>
          <GROSS>
            <xsl:choose>
              <xsl:when test="number($varGROSS)">
                <xsl:value-of select="format-number($varGROSS,'0.##')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
          </GROSS>

          <xsl:variable name="Commission1">
            <xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
          </xsl:variable>

		  <xsl:variable name="varCommission">
				<xsl:value-of select="format-number($Commission1,'0.##')"/>
		  </xsl:variable>
          <COMMS>
            <xsl:choose>
              <xsl:when test="number($Commission1)">
                <xsl:value-of select="$varCommission"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
          </COMMS>

          <CUSTFEE>
            <xsl:value-of select ="0"/>
          </CUSTFEE>

		<xsl:variable name="varSecFee">
			<xsl:value-of select="format-number(StampDuty,'0.##')"/>
		</xsl:variable>
          <SECFEES>
            <xsl:value-of select="$varSecFee"/>
          </SECFEES>
					<xsl:variable name="varNet">
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Close'">
								<xsl:value-of select="$Qty * $varAvgPrice + $varCommission"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell short'">
								<xsl:value-of select=" $Qty * $varAvgPrice - $varCommission - $varSecFee"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'0'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
          <NET>
             <xsl:value-of select="format-number($varNet,'0.##')"/>
          </NET>

          <xsl:variable name="varTradeDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="TradeDate"/>
            </xsl:call-template>
          </xsl:variable>
          <TRADDT>
            <xsl:value-of select="$varTradeDate"/>
          </TRADDT>

          <xsl:variable name="varSettlDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate"/>
            </xsl:call-template>
          </xsl:variable>
          <SETTLEDT>
            <xsl:value-of select="$varSettlDate"/>
          </SETTLEDT>

          <LOCATION>
            <xsl:value-of select="'US'"/>
          </LOCATION>
          
					<xsl:variable name="PB_NAME" select="'Lyrical'"/>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<BROKER>
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'JPM Securities LLC'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'Merrill Lynch Broadcort'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'Sanders Morris Harris LLC'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMBC'">
								<xsl:value-of select="'SMBC Nikko Securities America Inc'"/>
							</xsl:when>

							<xsl:when test="CounterParty='RJET'">
								<xsl:value-of select="'RJASUS3F'"/>
							</xsl:when>
							<xsl:when test="CounterParty='GS'">
								<xsl:value-of select="'GOLDMAN SACHS &amp; CO. LLC'"/>
							</xsl:when>

							<xsl:when test="CounterParty='BARC'">
								<xsl:value-of select="'Barclays'"/>
							</xsl:when>
						</xsl:choose>
					</BROKER>
          
          <TICKER>
            <xsl:value-of select="Symbol"/>
          </TICKER>

					
					<SECURITYDESCRIPTION>
						<xsl:value-of select="FullSecurityName"/>
					</SECURITYDESCRIPTION>

          <xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
          </xsl:variable>
            
          <xsl:variable name ="PB_FUND_Number">
	          <xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
          </xsl:variable>
            
          <AccountNumber>
            <xsl:choose>
              <xsl:when test="$PB_FUND_Number!=''">
                <xsl:value-of select="$PB_FUND_Number"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNumber>
          
          <AccountName>
            <xsl:choose>
              <xsl:when test="contains(AccountName,':')">
                <xsl:value-of select="substring-before(AccountName,':')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="AccountName"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountName>

					<EntityID>
						<xsl:value-of select="''"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
