<?xml version="1.0" encoding="UTF-8"?>
<!-- Description: MSFS_Sensato EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<Account>
					<xsl:value-of select="'Account'"/>
				</Account>

				<CustodianAccount>
					<xsl:value-of select="'Custodian Account'"/>
				</CustodianAccount>

				<ORKAccount>
					<xsl:value-of select="'ORK Account'"/>
				</ORKAccount>

				<!-- COL D-->
				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>


				<ValueDate>
					<xsl:value-of select="'Value Date'"/>
				</ValueDate>

				<CurrencyBought>
					<xsl:value-of select="'Currency Bought'"/>
				</CurrencyBought>

				<AmountBought>
					<xsl:value-of select="'Amount Bought'"/>
				</AmountBought>

				<CurrencySold>
					<xsl:value-of select="'Currency Sold'"/>
				</CurrencySold>

				<AmountSold>
					<xsl:value-of select="'Amount Sold'"/>
				</AmountSold>

				<TradeRate>
					<xsl:value-of select="'Trade Rate'"/>
				</TradeRate>

				<!-- COL K-->
				<BrokerName>
					<xsl:value-of select="'Broker Name'"/>
				</BrokerName>

				<SpotForward>
					<xsl:value-of select="'Spot/Forward'"/>
				</SpotForward>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			
			 <xsl:for-each select="ThirdPartyFlatFileDetail[Asset ='FX' or Asset ='FXForward']"> 
			<!-- <xsl:for-each select="ThirdPartyFlatFileDetail"> -->
			
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
					
				<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

				<xsl:variable name="THIRDPARTY_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='StoneCoast']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
				</xsl:variable>

				<xsl:variable name="AccountId_Alloc">
					<xsl:choose>
						<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
							<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PRANA_FUND_NAME"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

					<Account>
						<xsl:value-of select="$AccountId_Alloc"/>
					</Account>


					<CustodianAccount>
            <xsl:choose>
              <xsl:when test="AccountName='SG'">
                <xsl:value-of select="'7548600'"/>
              </xsl:when>
              <xsl:when test="AccountName='SG Equities'">
                <xsl:value-of select="'7548601'"/>
              </xsl:when>
              <xsl:when test="AccountName='KSSGFUT'">
                <xsl:value-of select="'7548603'"/>
              </xsl:when>
              <xsl:when test="AccountName='KSSGOPT'">
                <xsl:value-of select="'7548604'"/>
              </xsl:when>
              <xsl:when test="AccountName='KFSGFUT'">
                <xsl:value-of select="'7548605'"/>
              </xsl:when>
              <xsl:when test="AccountName='KSMSOPT'">
                <xsl:value-of select="'0331AA1N5'"/>
              </xsl:when>
              <xsl:when test="AccountName='MS Options'">
                <xsl:value-of select="'0331AATY1'"/>
              </xsl:when>
              <xsl:when test="AccountName='MS'">
                <xsl:value-of select="'052BAF280'"/>
              </xsl:when>
              <xsl:when test="AccountName='KSMSFUT'">
                <xsl:value-of select="'052BAHFZ2'"/>
              </xsl:when>
              <xsl:when test="AccountName='KPSGEOPT'">
                <xsl:value-of select="'C239458'"/>
              </xsl:when>
              <xsl:when test="AccountName='KSSGEOPT'">
                <xsl:value-of select="'C239524'"/>
              </xsl:when>
              <xsl:when test="AccountName='US Bank'">
                <xsl:value-of select="'US Bank'"/>
              </xsl:when>
              <xsl:when test="AccountName='KSUSB'">
                <xsl:value-of select="'US Bank'"/>
              </xsl:when>
            </xsl:choose>
					</CustodianAccount>

					<ORKAccount>
						<xsl:value-of select="''"/>
					</ORKAccount>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>


					<ValueDate>
						<xsl:value-of select="SettlementDate"/>
					</ValueDate>

					<CurrencyBought>
					<xsl:choose>
								<xsl:when test="substring-before(Symbol,'/')='AUD' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='GBP' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='EUR' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='NZD' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                <xsl:when test="substring-before(Symbol,'/')='AUD' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='GBP' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='EUR' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='NZD' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>		
                                <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                 <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>									
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						<!-- <xsl:value-of select="VsCurrencyName"/> -->
					</CurrencyBought>

					<AmountBought>
						<xsl:choose>
								<xsl:when test="Side='Buy'">
									<xsl:value-of select="AllocatedQty"/>
								</xsl:when>
								<xsl:when test="Side='Sell'">								
									<xsl:value-of select="AllocatedQty * AveragePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
					</AmountBought>

					<CurrencySold>
				<xsl:choose>
								<xsl:when test="substring-before(Symbol,'/')='AUD' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='GBP' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='EUR' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='NZD' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='AUD' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='GBP' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='EUR' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='NZD' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>
                                 <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Buy'">
									<xsl:value-of select="VsCurrencyName"/>
								</xsl:when>	
                                 <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                  <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                                   <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>	
                               <xsl:when test="substring-before(Symbol,'/')='USD' and Side='Sell'">
									<xsl:value-of select="LeadCurrencyName"/>
								</xsl:when>									
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						
					</CurrencySold>

					<AmountSold>
						<xsl:choose>
						<xsl:when test="Side='Sell'">
							<xsl:value-of select="AllocatedQty"/>
						</xsl:when>
						<xsl:when test="Side='Buy'">
							<xsl:value-of select="(AllocatedQty * AveragePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
					</AmountSold>

					<TradeRate>
						<xsl:value-of select="AveragePrice"/>
					</TradeRate>

					
					<BrokerName>
						<xsl:value-of select="''"/>
					</BrokerName>

					<SpotForward>
						<xsl:choose>
				<xsl:when test="Asset = 'FX'">
					<xsl:value-of select ="'S'"/>
				</xsl:when>
				<xsl:when test="Asset = 'FXForward'">
					<xsl:value-of select ="'F'"/>
				</xsl:when>
				<xsl:otherwise>
				<xsl:value-of select="Asset"/>
				</xsl:otherwise>
			</xsl:choose>
					</SpotForward>



					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
